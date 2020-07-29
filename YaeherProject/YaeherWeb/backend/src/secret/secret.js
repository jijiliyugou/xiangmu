import md5 from 'js-md5'
function getSecret(){
	let Base64 = require('js-base64').Base64;
    const nonce = parseInt(Math.random()*1000);
    const timestamp = Math.round(new Date()/1000);
    const apptype = 'pc'
    const secret1 = 'ds*^tgg)B5445'
    const secret2 = '988Fddgb@^78956$&'
    const signature1 = `apptype=${apptype}&nonce=${nonce}&secret1=${secret1}&secret2=${secret2}&timestamp=${timestamp}`
    const signature = md5(signature1)
    const string = `timestamp=${timestamp}&nonce=${nonce}&apptype=${apptype}&signature=${signature}`
    return Base64.encode(string)
}
export default{ getSecret };