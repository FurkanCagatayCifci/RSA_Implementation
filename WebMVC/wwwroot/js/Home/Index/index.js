import("/lib/jquery/dist/jquery.js");

$(
	function () {
		const fileInputBox = document.getElementById("fileInputBox");
		const fileUploadForm = document.getElementById("fileUploadForm");
		const submitResult = jQuery("#submit-result");
		const inputStatusDiv = jQuery("#inputStatusDiv");
		const inputStatusMessage = jQuery("#inputStatusMessage");
		const inputStatusWrapper = jQuery("#inputStatusDiv #wrapper");
		const cancel_dialog = jQuery("#cancel-dialog");
		const loadstatus = jQuery("#loadstatus");
		var formData = new FormData();
		var reader;
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
		function getFilesLength(formData) {
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
		function showSuccess() {
			submitResult.html("Dosyalar Baþarýyla Yüklendi");
		}
		function showError(reason) {
			submitResult.html(`<p>${reason}</p>`);
		}
		function encryptFileContent(content) {
			return Promise.resolve(import("/js/rsa.js")).then((val) => {
				const rsa = val;
				var encrypted = [];
				if (content instanceof File) {
					var i = 0;
					for (var c of content) {
						encrypted[i] = rsa.rsaEncrypt(c, api_key.x, api_key.n);
						encrypted[i + 1] = ",";
						i += 2;
					}
					return encrypted;
				}
				else if (content instanceof ProgressEvent) {
					var i = 0;
					for (var c of content.target.result) {
						encrypted[i] = rsa.rsaEncrypt(c, api_key.x, api_key.n);
						encrypted[i + 1] = ",";
						i += 2;
					}
					return encrypted;
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

		/**
		//async function postToAPI(formData) {
		//	var length = await getFilesLength(formData);
		//	if (length > 0) {
		//		$.ajax(
		//			"http://localhost:5044/api/fileUpload/upload",
		//			{
		//				method: "POST",
		//				enctype: "multipart/form-data",
		//				url: "http://localhost:5044/api/fileUpload/upload",
		//				headers: {
		//					"Access-Control-Allow-Origin": "*",
		//					"Access-Control-Allow-Methods": "POST,GET, OPTIONS",
		//					//"Content-Type": `multipart/form-data;boundary=${length}`,
		//				},
		//				crossDomain: true,
		//				cache: false,
		//				contents: formData,
		//				contentType: false,
		//				processData: false,
		//				data: formData,
		//				success: showSuccess,
		//				error: showError,
		//			}
		//		);
		//	}
		//	else {
		//	}
		//}
		**/
		var api_key;
		Promise.resolve(getApiKey()).then((val) => {
			api_key = val;
		});

		if (fileInputBox != null) {
			fileInputBox.addEventListener("change", function () {
				if (fileInputBox.files.length > 0) {
					if (getFilesLength(fileInputBox.files) > 1) {
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
							reader = new FileReader();
							if (file instanceof Blob) {
								const fileName = file.name;
								reader.readAsText(file, "utf-8");

								reader.onprogress = (event) => {
									inputStatusMessage.html("<p> Dosyalar Hazýrlanýyor </p>");

								};
								reader.onerror = async (event) => {
									inputStatusMessage.text(`<p> Dosya Yüklenemedi : ${file.name}</p>`);
								};
								reader.onload = (event) => {
									console.log(event);
									encryptFileContent(event).then((encrypted) => {
										formData.append(
											fileName,
											new File(encrypted, fileName,
												{ type: undefined }
											));
										inputStatusMessage.html("<p> Dosyalar Hazýr </p>");
									}).catch((r) => { console.log(r) });
								}
							}
						}
					}
					else {
						submitResult.text(`Geçersiz Dosya Boyutu : ${length}`);
						fileUploadForm.reset();
						fileInputBox.dispatchEvent(new Event("change"));
					}
					var intreval = function () {
						if (fileInputBox.files.length == formData.length) {
							clearInterval();
							clearTimeout();
						}
						else
							;
					};

					var timeOut = function () {
						setInterval(intreval, 3000);
					};
					setTimeout(timeOut, 3000);
				}
				else {
					inputStatusMessage.text("Lütfen bir dosya seçin.");
				}
			});
		}

		if (fileUploadForm != null) {
			fileUploadForm.addEventListener("submit",
				async function (event) {
					event.preventDefault();
					postToAPI(formData);
				});
		}
	}
);
