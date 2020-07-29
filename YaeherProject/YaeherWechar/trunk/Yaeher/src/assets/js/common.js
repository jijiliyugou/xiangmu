import md5 from 'js-md5'
import axios from 'axios'
let Base64 = require('js-base64').Base64;
import { Toast } from 'mint-ui';


function createSecret() { // secret生成
    const nonce = parseInt(Math.random()*1000);
    const timestamp = Math.round(new Date()/1000);
    const apptype = 'wx'
    const secret1 = 'ds*^tgg)B5445'
    const secret2 = '988Fddgb@^78956$&'
    const signature1 = `apptype=${apptype}&nonce=${nonce}&secret1=${secret1}&secret2=${secret2}&timestamp=${timestamp}`
    const signature = md5(signature1)
    const string = `timestamp=${timestamp}&nonce=${nonce}&apptype=${apptype}&signature=${signature}`
    return Base64.encode(string)
} 

function convertUTCTimeToLocalTime (UTCDateString, moth) { // 时间格式转换
    if(!UTCDateString){
      return '-';
    }
    function formatFunc(str) {    //格式化显示
      return str > 9 ? str : '0' + str
    }
    var date2 = new Date(UTCDateString);     //这步是关键
    var year = date2.getFullYear();
    var mon = formatFunc(date2.getMonth() + 1);
    var day = formatFunc(date2.getDate());
    var hour = date2.getHours();
    hour = formatFunc(hour);
    var min = formatFunc(date2.getMinutes());
    var dateStr = year+'-'+mon+'-'+day+' '+hour+':'+min;
    if (moth === 1) { // 月/日
        var dateStr = mon+'/'+day;
        return dateStr;
    } else if (moth === 2) { // 小时：分钟
        var dateStr = hour+':'+min;
        return dateStr;
    } else if (moth === 3) { // 年-月-日
        var dateStr = year+'-'+mon+'-'+day;
        return dateStr;    
    } else if (moth === 4) {
        var dateStr = mon;
        return dateStr;
    } else { // 年月日时分
        return dateStr;
    }
    
  }

function verifyPrice (verifyArray) {  // 校验空
    for(let i = 0 ; i < verifyArray.length; i++) {
        if (!verifyArray[i].value || verifyArray[i].value === -1) {
            Toast(verifyArray[i].msg)
            return 1
        }
    }
    
}

function add0 (m) {
    return m < 10 ? `0${m}` : m
}

function timestamp () {
    var time = new Date()
    var y = time.getFullYear()
    var m = time.getMonth() + 1
    var d = time.getDate()
    var h = time.getHours()
    var mi = time.getMinutes()
    var s = time.getSeconds()
    var aTanisi = Math.floor(Math.random() * 999999)
    return `${y}${add0(m)}${add0(d)}${add0(h)}${add0(mi)}${add0(s)}${aTanisi}`
}

function formatDate(date, yearshow) {
    console.log(date)
    const y = date.getFullYear()
    let m = date.getMonth() + 1
    m = m < 10 ? '0' + m : m
    let d = date.getDate()
    d = d < 10 ? ('0' + d) : d
    let lastDay  = new Date(y, m, 0)
    lastDay = lastDay.getDate()
    if (yearshow===0) {
        let dataTime = y + '-' + m + '-' + d
        let timeObj = {
            dataTime: dataTime,
            startTime: dataTime,
            endTime: dataTime
        }
        return timeObj
    } else if (yearshow===1) {
        let dataTime = y + '-' + m
        let timeObj = {
            dataTime: dataTime,
            startTime: `${dataTime}-01`,
            endTime: `${dataTime}-${lastDay}`
        }
        return timeObj
    } else {
        let dataTime = y
        let timeObj = {
            dataTime: dataTime,
            startTime: `${dataTime}-01-01`,
            endTime: `${dataTime}-12-31`
        }
        return timeObj
    }
    
}

function dataURLtoBlob(dataurl) { // base64转blob
    var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }
    return new Blob([u8arr], { type: mime });
}

function compress(img, Orientation) {
    let canvas = document.createElement("canvas");
    let ctx = canvas.getContext('2d');
    ctx.fillStyle = "#fff";
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    if(Orientation != "" && Orientation != 1){
        switch(Orientation){
            case 6://需要顺时针（向左）90度旋转
                this.rotateImg(img,'left',canvas);
                break;
            case 8://需要逆时针（向右）90度旋转
                this.rotateImg(img,'right',canvas);
                break;
            case 3://需要180度旋转
                this.rotateImg(img,'right',canvas);//转两次
                this.rotateImg(img,'right',canvas);
            break;
        }
    }   
    let ndata = canvas.toDataURL('image/jpeg', 1) 
    console.log('ndata', ndata)
    return ndata;
}

function fontVery (string) {
    let reg = /[^\u0020-\u007E\u00A0-\u00BE\u2E80-\uA4CF\uF900-\uFAFF\uFE30-\uFE4F\uFF00-\uFFEF\u0080-\u009F\u2000-\u201f\u2026\u2022\u20ac\r\n]/g
    let res = reg.test(string);
    if (res) Toast('不能输入表情');
    let resFlag = !res;
    return resFlag
}

function rotateImg(img, direction,canvas) {
    //最小与最大旋转方向，图片旋转4次后回到原方向    
    console.log('rotateImg')
    const min_step = 0;    
    const max_step = 3;      
    if (img == null)return;    
    //img的高度和宽度不能在img元素隐藏后获取，否则会出错    
    let height = img.height;    
    let width = img.width;      
    let step = 2;    
    if (step == null) {    
        step = min_step;    
    }    
    if (direction == 'right') {    
        step++;    
        //旋转到原位置，即超过最大值    
        step > max_step && (step = min_step);    
    } else {    
        step--;    
        step < min_step && (step = max_step);    
    }     
    //旋转角度以弧度值为参数    
    let degree = step * 90 * Math.PI / 180;    
    let ctx = canvas.getContext('2d');    
    console.log(step,'step')
    switch (step) {    
    case 0:    
        canvas.width = width;    
        canvas.height = height;    
        ctx.drawImage(img, 0, 0);    
        break;    
    case 1:    
        canvas.width = height;    
        canvas.height = width;    
        ctx.rotate(degree);    
        ctx.drawImage(img, 0, -height);    
        console.log('over')
        break;    
    case 2:    
        canvas.width = width;    
        canvas.height = height;    
        ctx.rotate(degree);    
        ctx.drawImage(img, -width, -height);    
        break;    
    case 3:    
        canvas.width = height;    
        canvas.height = width;    
        ctx.rotate(degree);    
        ctx.drawImage(img, -width, 0);    
        break;
    }    
}
function hasClass1(element, cls) {
    return (' ' + element.className + ' ').indexOf(' ' + cls + ' ') > -1;
   }

export { 
    createSecret,
    convertUTCTimeToLocalTime,
    verifyPrice,
    timestamp,
    formatDate,
    compress,
    rotateImg,
    dataURLtoBlob,
    hasClass1,
    fontVery
}