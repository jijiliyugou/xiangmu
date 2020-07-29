<template>
    <header class="bbs-header">
        <div class="header-left">
           <img src="./../assets/images/yeaherlogo.png" alt="yeaher" class="header-logo">
            <a href="javascript:void(0)" class="cms-slide-menu" @click="collapseMenu"></a>
        </div>
         <div class="header-right">
             <div class="header-user-group">
                <img :src="userImage" alt="" class="user-logo">
                 <span class="username">{{userName}}</span>
                 </div>
                 <!-- <span title="修改密码" class="iconfont icon-bianji-copy" @click="showPwd"></span> -->
                <span title="退出"  class="iconfont icon-out-copy rightmargin" @click="logout"></span>
         </div>
    </header>
</template>
<script>
export default {
  name: "yh-header",
  data() {
    return {
      userName:localStorage.getItem('loginName'),
      siteItems:this.$store.state.perms.siteItems,//站点id
      siteId: localStorage.getItem('_site_id_param'),
      userImage:localStorage.getItem('userImage')=='null'?'./../static/images/userlogo.png':localStorage.getItem('userImage')
    };
  },
  computed:{
       siteName(){
          for(let i in this.siteItems){
              if(this.siteItems[i].id==this.siteId){
                return this.siteItems[i].name;
                break;
              }
          }
       }
  },
  methods: {
    setSiteId(val){
      localStorage.setItem('_site_id_param',val);
      this.$router.push("/work");
      window.location.reload();
      // window.location.reload();//重新处理请求
    },
    collapseMenu() {//侧边栏收缩
    this.$store.dispatch("setCollapse");
    },
    showPwd(){
      if(this.$store.state.sys.pwdCollapse){
          this.$store.dispatch('setPwd',false);
      }else{
        this.$store.dispatch('setPwd',true);
      }

    },
    toggleLocal(){

    },
    logout() {//退出登录
      this.$confirm('确定退出吗？','提示', {type: "warning"})
        .then(mes => {
          this.$store.dispatch("loginOut").then(res => {
              if(res.code=='200'){
                   this.$router.push("/login");
              }else{
                  this.errorMessage()
              }
          });
        })
        .catch(error => {});
    }
  }
};
</script>

 <style lang="scss" scoped>
$header-height: 60px;
.bbs-header {
  width: 100%;
  height: $header-height;
  background: #fff;
  border-bottom: 1px solid #e0e4e9;
  box-shadow: 10px 1px 15px rgba(0, 0, 0, 0.17);
  padding: 0 24px;
  position: fixed;
   position: fixed;
   top:0;
   z-index:1002;
   -webkit-transform: translateZ(0);

}
.header-left {
  float: left;
  display: flex;
  align-items: center;
  height: $header-height;
  width: 220px;
}
.header-right {
  float: right;
  display: flex;
  align-items: center;
  height: $header-height;
  .iconfont {
    cursor: pointer;
    font-size: 20px;
    color: #777777;
    margin-left: 25px;
    &:hover{
        color: #188ae2;
    }
  }
}

@media screen and(max-width:700px) {
  .header-right {
    display: none;
  }
  .header-left {
    width: 100%;
    .header-logo {
      text-align: center;
      margin: 0 auto;
    }
  }
}
.header-user-group {
  display: flex;
  height: $header-height;
  line-height: $header-height;
  align-items: center;
}
.user-logo {
  width: 32px;
  height: 32px;
  margin-right: 10px;
  border-radius:100%;
  overflow:hidden;
}

.header-logo {
  margin-left: 26px;
}
.cms-slide-menu {
  height: 16px;
  width: 18px;
  background: url("./../assets/images/collapse.png") no-repeat;
  margin-left: 26px;
}
.cur{
  cursor: pointer;
}
.rightmargin{margin-right:24px;}

</style>
