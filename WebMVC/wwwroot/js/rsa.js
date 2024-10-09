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
	var encrypted = [];
	for (var i in message) {
		encrypted[i] = modExp(message.charCodeAt(i), e, n);
	}


	return encrypted;
}
