import store from '@/store/index'
import { Encrypt } from "@/untils/aes";
import $http from "@/api/fetch";
import api from '@/api/api';
import { routes, ansycRoutes } from '@/router/routes'
import body from '@/components/body'
import childView from '@/components/appMain.vue'
/*
 * 
 * @param {异步路由表} ansycRoutes 
 * @param {数据库权限拉取} perms 
 */


/**
 * 递归处理角色权限
 */
/*function getansycRoutes(tmpRoutes, perms) {
     let aa=perms.split(',');
    const result = tmpRoutes.filter(route => {
        if (aa.indexOf(route.path) != -1) {
            if (route.children != undefined) {
                   let cc=aa.join(',');
                route.children = getansycRoutes(route.children,cc);
            }
            return true;
        }
        return false;
    }
    )
    return result
}*/
//路由封装函数
function arrTree(arr){
  const newArr=[];
  for(let i=0;i<=arr.length-1;i++){
     const obj={};
     obj.path=arr[i].linkUrls;
     obj.name=arr[i].names;
     obj.component=body;
     obj.iconCls=arr[i].icons;
     if(arr[i].children.length>0){
       obj.children=[];
       for(let j=0;j<=arr[i].children.length-1;j++){
         const childA={};
         childA.path=arr[i].children[j].linkUrls;
         childA.name=arr[i].children[j].names;

         if(arr[i].children[j].children.length>0){
           childA.component=childView;
           childA.isParent=true;
           childA.redirect=arr[i].children[j].linkUrls+"/list";
           obj.children.push(childA);
           obj.children[j].children=[];
           for(let k=0;k<=arr[i].children[j].children.length-1;k++){
             const childB={};
             childB.path=arr[i].children[j].children[k].linkUrls;
             childB.name=arr[i].children[j].children[k].names;
             childB.component=resolve => { require(['@/views'+arr[i].linkUrls+arr[i].children[j].children[k].linkUrls+'.vue'], resolve) };
             childB.hidden=true;
             obj.children[j].children.push(childB);
           }

         }else{
           childA.component=resolve => { require(['@/views'+arr[i].url+arr[i].children[j].linkUrls+'.vue'], resolve) };
           childA.hidden=true;
           obj.children.push(childA);
         }
       }
     }
     newArr.push(obj);
  }
  return newArr;
}
const perm = {
    state: {
        routers: routes,
        addRouters: [],
        perms: false,//判断是否刷新过页面,
        //permsList: null,
        siteItems:[],
        baseUrl:''
    },
    mutations: {

        SET_ROUTERS: (state, obj) => {
            state.routers = routes.concat(obj.asRouters);//设置路由表
            state.addRouters = obj.asRouters;//动态路由表
            state.perms = true;//登录状态
            //state.permsList = obj.permsList;//权限字符串 
            state.baseUrl=obj.baseUrl 
            /*let arr=[];
            for (let i in obj._site_id_param){//站点id 数组
                arr.push(obj._site_id_param[i].id);
            } */  
            /*let local_site_id_param = localStorage.getItem('_site_id_param');
            if (local_site_id_param){
                let isBeing = arr.indexOf(parseInt(local_site_id_param))            
                if (isBeing>=0){
                    
                    localStorage.setItem('_site_id_param',local_site_id_param);
                }else{
                    
                    localStorage.setItem('_site_id_param', state._site_id_param);
                }
            }else{
              
                localStorage.setItem('_site_id_param', state._site_id_param);
            } */   

        },
        CLEAR_ROUTERS: (state) => {
            state.routers = routes;
            state.addRouters = [];
            state.perms = false;
            //state.permsList = null;
            localStorage.clear();
            window.location.reload();//此处退出vuex状态不刷新，目前发现重载页面可以处理。。
        }
    },
    actions: {
        setRouters({ commit }) {
            return new Promise((resolve, reject) => {//动态加载路由权限
                $http.post(api.getPerms,{id:localStorage.getItem('userId')})
                    .then(res => {
                        if(res.result.code==200){
                            let asRouters;
                            //asRouters = ansycRoutes;
                            if(res.result.item.moduleList!=null){
                                asRouters = arrTree(res.result.item.moduleList);
                            }else{
                                asRouters =[];
                            }
                            commit('SET_ROUTERS', {
                                asRouters: asRouters
                            });
                            const roleList=res.result.item.roleList;//用户角色
                            if(roleList==null){
                                return false;
                            }
                            if(roleList.length>0){
                                var roleArr=roleList.map(function(data){
                                    return data.roleCode;
                                });
                            }
                            localStorage.setItem('loginName',res.result.item.yaeherUserInfo.loginName);
                            localStorage.setItem('role',JSON.stringify(roleArr));
                            resolve(res);
                        }
                    }).catch(error => {
                        reject(error);
                    })
            })
        },
        loginOut({ commit }) {
            commit('CLEAR_ROUTERS');
            /*return new Promise((resolve, reject) => {
                $http.post(api.loginOut).then(res=>{
                    if(res.code=='200'){
                        commit('CLEAR_ROUTERS');
                    } 
                    resolve(res);
                }).catch(error=>{
                    reject(error);
                })     
            })*/
        }
    }

}

export default perm