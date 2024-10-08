import("/lib/jquery/dist/jquery.js");


$(
	async function () {
		const fileInputBox = document.getElementById("fileInputBox");
		const fileUploadForm = document.getElementById("fileUploadForm");
		const submitResult = jQuery("#submit-result");
		const inputStatusDiv = jQuery("#inputStatusDiv");
		const inputStatusMessage = jQuery("#inputStatusMessage");
		const inputStatusWrapper = jQuery("#inputStatusDiv #wrapper");
		const cancel_dialog = jQuery("#cancel-dialog");
		const loadstatus = jQuery("#loadstatus");
		var formData = new FormData();
		const reader = new FileReader();


		async function getApiKey() {
			return await $.ajax("http://localhost:5044/api/getKey",
				{
					method: "GET",
					success: (data) => {
						return data;
					}
				}
			);
		}
		async function getFilesLength(formData) {
			var length = 0;
			if (formData instanceof FileList) {
				for (var i of formData) {
					length += i.size;
				}
			}
			else if (formData instanceof FormData) {
				for (var i of formData) {
					length += i.length;
				}
			}
			return length;
		}
		async function showSuccess() {
			submitResult.html("Dosyalar Baþarýyla Yüklendi");
		}
		async function showError(reason) {
			submitResult.html(`<p>${reason}</p>`);
		}

		// Basit bir þifreleme fonksiyonu deneme
		/**
		 * 
		 * @argument content  
		 * */

		async function encryptFileContent(content) {
			const rsa = await import("/js/rsa.js");
			var encrypted = [];
			var i = 0;
			for (var c of content) {
				encrypted[i] = rsa.rsaEncrypt(c, api_key.x, api_key.n);
				encrypted[i + 1] = ",";
				i += 2;
			}
			return encrypted;
		}
		async function postToAPI(formData) {
			var length = await getFilesLength(formData);
			if (length > 0) {
				$.ajax(
					"http://localhost:5044/api/fileUpload/upload",
					{
						method: "POST",
						enctype: "multipart/form-data",
						url: "http://localhost:5044/api/fileUpload/upload",
						headers: {
							"Access-Control-Allow-Origin": "*",
							"Access-Control-Allow-Methods": "POST,GET, OPTIONS",
							//"Content-Type": `multipart/form-data;boundary=${length}`,
						},
						crossDomain: true,
						cache: false,
						contents: formData,
						contentType: false,
						processData: false,
						data: formData,
						success: showSuccess,
						error: showError,
					}
				);
			}
			else {
			}

		}

		var api_key = await getApiKey();

		if (fileInputBox != null) {
			fileInputBox.addEventListener("change", async function () {
				if (fileInputBox.files.length > 0) {
					if (await getFilesLength(fileInputBox.files) > 1) {
						if (fileInputBox.files.length > 1) {
							inputStatusMessage.text("Seçilen Dosyalar");
							var fileNames = "";
							for (var file of fileInputBox.files) {
								fileNames = fileNames + `<p>${file.name}</p>`;
							}
							inputStatusWrapper.html(`${fileNames}`);
						} else if (fileInputBox.files.length == 1) {
							const fileName = fileInputBox.files[0].name;
							inputStatusMessage.text(`Seçilen dosya : ${fileName}`);
							inputStatusWrapper.html("");
						}
						for (var file of fileInputBox.files) {
							if (file instanceof Blob) {
								reader.readAsBinaryString(file);
							}
							reader.onerror = async () => {
								inputStatusMessage.html(`<p> Dosya Yüklenemedi : ${file.name}</p>`);
							}
							reader.onload = async () => {
								var encryptedContent = await encryptFileContent(reader.result);
								console.log("e: " + api_key.x + " n: " + api_key.n);
								console.log("fileName: " + file.name);
								console.log("content : " + reader.result);
								console.log("chiper : " + encryptedContent);
								formData.append(
									file.name,
									new File(encryptedContent, file.name,
										{ type: undefined }
									));
							}
						}
					}
					else {
						submitResult.text(`Geçersiz Dosya Boyutu : ${length}`);
						fileUploadForm.reset();
						fileInputBox.dispatchEvent(new Event("change"));
					}
				}
				else {
					inputStatusMessage.text("Lütfen bir dosya seçin.");
				}
			});
		}

		if (fileUploadForm != null) {
			fileUploadForm.addEventListener("submit",
				async function (event) {
					if (reader.DONE) {
						event.preventDefault();
						postToAPI(formData);
					}
					else {
						while (reader.LOADING) {
						};
					}
				}
			);
		}
		async function postToAPI(formData) {
			var length = await getFilesLength(formData);
			if (length > 0) {
				$.ajax(
					"http://localhost:5044/api/fileUpload/upload",
					{
						method: "POST",
						enctype: "multipart/form-data",
						url: "http://localhost:5044/api/fileUpload/upload",
						headers: {
							"Access-Control-Allow-Origin": "*",
							"Access-Control-Allow-Methods": "POST,GET, OPTIONS",
							//"Content-Type": `multipart/form-data;boundary=${length}`,
						},
						crossDomain: true,
						cache: false,
						contents: formData,
						contentType: false,
						processData: false,
						data: formData,
						success: showSuccess,
						error: showError,
					}
				);
			}
			else {
			}

		}
	}
);
