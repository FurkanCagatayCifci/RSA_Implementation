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
		var reader = new FileReader();

		async function getMaxFileLength() {
			return await $.ajax("http://localhost:5044/api/maxFileLength",
				{
					method: "GET",
					success: (data) => {
						return data;
					}
				}
			);
		}
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
		function getFilesLength(data) {
			var length = 0;
			if (data instanceof FileList) {
				for (var i of data) {
					length += i.size;
				}
			}
			else if (data instanceof FormData) {
				for (var i of data) {
					length += i.length;
				}
			}
			else if (data instanceof File) {
				length += data.size
			}
			else if (data instanceof Array) {
				for (var i of data) {
					length += i.size;
				}
			}
			return length;
		}
		function showSuccess() {
			submitResult.html("<p>Dosyalar Baþarýyla Yüklendi</p>");
		}
		function showError(reason) {
			console.log(reason);
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
		function displayFormData(files) {
			if (files.length > 1) {
				inputStatusMessage.text("Seçilen Dosyalar");
				var html = "";
				for (var file of files) {
					html = html + `<p>${file.name}</p>`;
				}
				inputStatusWrapper.html(`${html}`);
				inputStatusDiv.html(inputStatusMessage[0].outerHTML + inputStatusWrapper[0].outerHTML + `<p>Toplam Boyut : ${getFilesLength(files)} byte`);
			} else if (files.length == 1) {
				const fileName = files[0].name;
				inputStatusMessage.html(`<p>Seçilen dosya : ${fileName}<p>`);
				inputStatusWrapper.html("");
				inputStatusDiv.html(inputStatusMessage[0].outerHTML + inputStatusWrapper[0].outerHTML);
			}
		}


		var api_key;
		Promise.resolve(getApiKey()).then((val) => {
			api_key = val;
		});
		var maxFileLength;
		Promise.resolve(getMaxFileLength()).then((val) => {
			maxFileLength = val;
		});

		if (fileInputBox != null) {
			fileInputBox.addEventListener("change", function () {
				formData = new FormData();
				var files = Array.from(fileInputBox.files);
				if (files.length > 0) {
					if (getFilesLength(files) > 1) {
						for (var i in files) {
							var file = files[i];
							reader = new FileReader();
							if (file instanceof Blob) {
								const fileName = file.name;
								files = files.filter(item => getFilesLength(item) < maxFileLength);
								if (getFilesLength(file) > maxFileLength) {
									inputStatusMessage.html(`Dosya uzunluðu maksimum ${maxFileLength} byte olmalý`);
								}

								reader.onprogress = (event) => {
									inputStatusMessage.html("<p> Dosyalar Hazýrlanýyor </p>");
								};
								reader.onerror = (event) => {
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
									}).catch((r) => { console.log(r) });
								}
								reader.readAsText(file, "utf-8");
								displayFormData(files);
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
