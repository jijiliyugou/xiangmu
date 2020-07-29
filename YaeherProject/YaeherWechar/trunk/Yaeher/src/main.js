// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
// import 'babel-polyfill'
import MintUI from 'mint-ui'
import 'mint-ui/lib/style.css'
import App from './App'
import router from './router'
// import axios from 'axios'
import 'font-awesome/css/font-awesome.css'
import instance_ from 'assets/js/index.js'
import gallery from 'img-vuer'
// import FastClick from 'fastclick'
// import vuePicturePreview from 'vue-picture-preview'
import preview from 'vue-photo-preview'
import 'vue-photo-preview/dist/skin.css'
Vue.use(preview)
// Vue.use(vuePicturePreview)
Vue.use(gallery)
// FastClick.attach(document.body)


import 'assets/sass/base.scss'
Vue.config.productionTip = false
Vue.use(MintUI)
// axios.defaults.baseURL = 'http://192.168.2.147:'
// Vue.prototype.$http = axios
Vue.prototype.instance = instance_

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})
