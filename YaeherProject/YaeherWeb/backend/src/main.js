// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import App from './App'//根组件
import router from './router'//vue-router
import store from "./store"//vuex
import "babel-polyfill";
import './permissions'//全局路由钩子
import '~/css/index.scss'//样式
import global from "@/untils/global"//全局方法
import cmsComponents from "@/components/index";
import '@/directive/index'
import Viewer from 'v-viewer'
import 'viewerjs/dist/viewer.css'
Vue.use(cmsComponents);
Vue.use(ElementUI);
Vue.use(Viewer, {
    defaultOptions: {
       "inline": false, "button": true, "navbar": true, "title": false, "toolbar": true, "tooltip": false, "movable": true, "zoomable": true, "rotatable": true, "scalable": false, "transition": false, "fullscreen": false, "keyboard": true, "url": "data-source"
    }
});

Vue.config.productionTip = false
Vue.use(global);
new Vue({
  router,
  store,
  template: '<App/>',
  components: { App }
}).$mount('#app')
