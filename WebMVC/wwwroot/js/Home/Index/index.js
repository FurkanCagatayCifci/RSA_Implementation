import("jquery");
$(

	function () {

		var fileInputBox = document.getElementById("#fileInputBox");
		var fileUploadForm = document.getElementById("fileUploadForm");
		var inputStatusDiv = jQuery("#inputStatusDiv");
		var inputStatusMessage = jQuery("#inputStatusMessage");
		var inputStatusWrapper = jQuery("#inputStatusMessage #wrapper");
		console.log(inputStatusWrapper);
		console.log(fileInputBox, inputStatusMessage, fileUploadForm);
		if (fileInputBox != null) {
			fileInputBox.addEventListener("change", function () {
				if (fileInputBox.files.length > 1) {
					inputStatusMessage.textContent = "Seçilen Dosyalar";
					var fileNames = "";
					for (var file of fileInputBox.files) {
						fileNames = fileNames + `<p>${file.name}</p>`;
					}
					inputStatusWrapper.innerHTML = `${fileNames}`;
				} else if (fileInputBox.files.length == 1) {
					const fileName = fileInputBox.files[0].name;
					inputStatusMessage.textContent = `Seçilen dosya : ${fileName}`;
				}
				else {
					inputStatusMessage.textContent = "Lütfen bir dosya seçin.";
				}
			});
		}
		if (fileUploadForm != null) {
			fileUploadForm.addEventListener("submit", function (event) {

				event.preventDefault();

				if (!fileInputBox.files.length) {
					inputStatusMessage.textContent = "Lütfen bir dosya seçin.";
					return;
				}

				const reader = new FileReader();


				reader.onload = function (event) {
					const fileContent = event.target.result;

					// Þifreleme iþlemi API den gelen key ile yapilacak
					const encryptedContent = encryptFileContent(fileContent);
					downloadEncryptedFile(encryptedContent, Date.now());

					/**
					// Þifrelenmiþ içeriði yeni bir dosya olarak kaydet
					//downloadEncryptedFile(encryptedContent, file.name.replace('.txt', '_encrypted.txt'));
					//inputStatusMessage.textContent = `Dosya "${file.name}" baþarýyla þifrelendi ve indiriliyor.`;
			
					*/

					$.ajax("http://localhost:5044",
						{
							method: "post",
							data: encryptedContent,
							error: alert("Failure on Upload"),
							success: alert("File uploads Successfull")
						}
					);
				};


			});
		}
		/**
		// Basit bir þifre çözme fonksiyonu deneme
		function decryptFileContent(encryptedContent) {
			return encryptedContent.split('').reverse().join(''); // Örnek þifre çözme: Ters çevrili içeriði geri çeviriyoruz.
		}
		
		// Þifrelenmiþ içeriði yeni bir dosya olarak kaydetme
		**/

		/**
		// Çözümlenen içeriði yeni bir dosya olarak kaydetme
		
		function downloadDecryptedFile(content, fileName) {
			const blob = new Blob([content], { type: 'text/plain' });
			const url = URL.createObjectURL(blob);
			const a = document.createElement('a');
			a.href = url;
			a.download = fileName;
			document.body.appendChild(a);
			a.click();
			document.body.removeChild(a);
			URL.revokeObjectURL(url);
		}
		
		**/

		/**
		// Çözümleme butonuna týklandýðýnda
		document.getElementById("decryptButton").addEventListener("click", function () {
			// Eðer dosya seçilmemiþse uyarý ver
			if (!fileInputBox.files.length) {
				inputStatusMessage.textContent = "Lütfen bir dosya seçin.";
				return;
			}
		
			const file = fileInputBox.files[0];
			const reader = new FileReader();
		
		
			reader.onload = function (event) {
				const encryptedContent = event.target.result;
				console.log("Þifreli dosya içeriði:", encryptedContent);
		
				// Þifre çözme iþlemi
				const decryptedContent = decryptFileContent(encryptedContent);
				//  yeni bir dosya olarak kaydet
				downloadDecryptedFile(decryptedContent, file.name.replace('.txt', '_decrypted.txt'));
			};
		
		
			reader.readAsText(file);
		});
		
		**/

		// Basit bir þifreleme fonksiyonu deneme
		function encryptFileContent(content) {
			// TODO
		}

		function downloadEncryptedFile(content, fileName) {
			const blob = new Blob([content], { type: 'text/plain' });
			const url = URL.createObjectURL(blob);
			const a = document.createElement('a');
			a.href = url;
			a.download = fileName;
			document.body.appendChild(a);
			a.click();
			document.body.removeChild(a);
			URL.revokeObjectURL(url);
		}

	}
);