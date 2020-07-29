import store from '@/store/index'
import $http from "@/api/fetch";
import api from '@/api/api';
import { Encrypt } from "@/untils/aes";
import md5 from 'js-md5'

/**
 * 登录，退出操作
 * 
 **/
const user = {
    state: {
        userNameOrEmailAddress:'none',
    },
    mutations: {
        LOGING_STATE: (state, params) => {
            //localStorage.setItem('sessionKey',params);
            state.userName = localStorage.getItem('userNameOrEmailAddress');
                
        }
    },
    actions: {
        userLogin({ commit }, params) {//登录异步操作
              let userName=params.userNameOrEmailAddress;
              params.password=md5(params.password);//加密
                return new Promise((resolve, reject) => {
                    $http.post(api.userLogin, params)
                    .then(res => {
                        if (res.result.code == 200) {
                            localStorage.setItem('userNameOrEmailAddress',userName);
                            localStorage.setItem('userToken',res.result.item.accessToken);
                            localStorage.setItem('userId',res.result.item.userId);
                            localStorage.setItem('userImage',res.result.item.userManager.yaeherUserInfo.userImage);
                            if(res.result.item.userManager.mobileRoleName=='doctor'){
                               localStorage.setItem('doctorId',res.result.item.userManager.yaeherDoctorInfo.id); 
                            }
                            const roleList=res.result.item.userManager.roleList;//用户角色
                            if(roleList==null){
                                return false;
                            }
                            if(roleList.length>0){
                                var roleArr=roleList.map(function(data){
                                    return data.roleCode;
                                });
                            }
                            localStorage.setItem('role',JSON.stringify(roleArr))
                            ;
                            commit('LOGING_STATE');
                        }
                        resolve(res)
                    }).catch(error => {
                        reject(error);
                    })
                })
        },
    }
}

export default user