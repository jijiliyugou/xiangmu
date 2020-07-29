import axios from 'axios'
import qs from 'qs'
import store from '../store/index'
import router from '../router/index'
import { Message } from 'element-ui'
import secretpush from '@/secret/secret'
function showMessage(value) {
    return Message({
        showClose: true,
        message: value,
        type: 'error',
        duration: 3500
    });
}
/*axios.defaults.retry = 4;
axios.defaults.retryDelay = 100;*/
axios.defaults.timeout = 25000;

/**
 * 请求拦截器
 * 
 */
axios.interceptors.request.use(config => {//在此处统一配置公共参数
    let userToken =localStorage.getItem('userToken')
    let secret=secretpush.getSecret();
    if (userToken) {
        userToken = 'Bearer ' + userToken;
        config.headers.Authorization = userToken;
    }
    let params = {
        secret:secret,//secret
        Platform:'PC',//平台类型
    };
    for (let key in config.data) {
        params[key] = config.data[key]; //添加进参数列表
    }
    config.data = /*qs.stringify(*/params/*)*/;//序列化
    return config;
}, error => {
  
    Promise.reject(error);// 错误提示
})


/**响应拦截器 */
axios.interceptors.response.use(response => {
    let res = response.data.result;
    switch (res.code) {
        case 200:
          
            break;
        case 204:
          
            break;
        case 301:
            
            break;
        case 304:
           
            break;
        case 3:
            showMessage(res.code + ":" + res.msg);
            localStorage.clear();
            router.push('/login');
            store.state.perms.perms=false;
            window.location.reload();
            break;
        case 302:
            showMessage(res.code + ":" + res.msg);
            localStorage.clear();
            router.push('/login');
            store.state.perms.perms=false;
            window.location.reload();
            break;
        case 401:
            showMessage(res.code + ":" + res.msg);
            localStorage.clear();
            router.push('/login');
            store.state.perms.perms=false;
            window.location.reload();
            break;
        default:
            showMessage(res.code + ":" + res.msg);
            break;
    }
    return response.data;
},
    error => {
        /*if(! error.config || ! error.config.retry) {
            return Promise.resolve(error.response)
        }
        error.config.__retryCount = error.config.__retryCount || 0
        if(error.config.__retryCount >= error.config.retry) {
            return Promise.resolve(error.response)
        } else {
            error.config.__retryCount += 1
        }
        var backoff = new Promise(resolve => {
            setTimeout(() => {
                resolve()
            }, error.config.retryDelay)
        })
        return backoff.then(r => {
            return axios(error.config)
        })*/
        if(error && error.response){
            switch (error.response.status) {
                case 401:
                    //showMessage('用户未登录，请先登录!');
                    localStorage.clear();
                    router.push('/login');
                    store.state.perms.perms=false;
                    window.location.reload();
                    break;
                default:
                    showMessage('服务器响应失败');
                    break;
            }
        }
        /*showMessage('服务器响应失败');
       
        return Promise.reject(error)*/
    }
);

export default axios
