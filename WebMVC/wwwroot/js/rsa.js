// Function to perform modular exponentiation (m^e mod n)
function modExp(base, exp, mod) {
	let result = 1;
	base = base % mod;  // Make sure base is smaller than mod
	while (exp > 0) {
		if (exp % 2 === 1) {  // If exp is odd, multiply base with result
			result = (result * base) % mod;
		}
		exp = Math.floor(exp / 2);  // Divide the exponent by 2
		base = (base * base) % mod;  // Square the base
	}
	return result;
}

// RSA encryption function
export function rsaEncrypt(message, e, n) {
	// Convert the message into an integer (simple ASCII conversion)
	//let m = 0;
	//for (let i = 0; i < message.length; i++) {
	//	m = m * 256 + message.charCodeAt(i);
	//}

	// Ensure that the message as a number is less than n
	//if (m >= n) {
	//	throw new Error("Message is too long for the provided modulus n.");
	//}

	// Encrypt the message using the formula: c = m^e mod n
	var encrypted = [];
	for (var i in message) {
		encrypted[i] = modExp(message.charCodeAt(i), e, n);
	}


	return encrypted;
}
