<template>
  <div class="page">
      <div class="loginbox">
         <h6>怡禾健康</h6>
         <div class="inputbox"><el-input v-model="params.userNameOrEmailAddress" @blur="requireName" placeholder="请输入用户名" class="user" @keyup.enter.native="enterin"></el-input><label class="labeltip" id="usererror"></label></div>
         <div class="inputbox"><el-input type="password" v-model="params.password" @blur="requirePass" placeholder="请输入密码" class="user" @keyup.enter.native="enterin"></el-input><label class="labeltip" id="passerror"></label></div>
         <p class="errortip" id="errorTip"></p>
         <el-button :loading="loading" type="success" class="login-btn" @click="login">{{logindec}}</el-button>
      </div>
  </div>
</template>
<script>
export default {
  name: 'login',
  data () {
    return {
      loading:false,
      params:{
         userNameOrEmailAddress:'',
         password:'',
      },
      logindec:"登录"
    }
  },
  methods:{
     getImgRez(){//获取上传类型
        this.$http.post(this.$api.getType).then(res=>{
          localStorage.setItem('imRez',res.result.item.imageThumNail[0].value);
        });
     },
     requireName(){
        if(this.params.userNameOrEmailAddress==""){
          $("#usererror").addClass("error").text("用户名不能为空");
        }else{
          $("#usererror").removeClass("error").text("");
        }
     },
     requirePass(){
        if(this.params.password==""){
          $("#passerror").addClass("error").text("密码不能为空");
        }else{
          $("#passerror").removeClass("error").text("");
        }
     },
     enterin(e){
        var keyCode = window.event? e.keyCode:e.which;
        if(keyCode == 13){
          this.login();
        }
     },
     login(){
       if(this.params.userNameOrEmailAddress=="" || this.params.password==""){
         $("#errorTip").text("");
         this.requireName();
         this.requirePass();
       }else{
        this.loading=true;
        this.logindec="正在登陆...";
         this.$store.dispatch("userLogin",this.params).then(res=>{
             if(res.result.code==200){
                this.$router.push("/work");
                this.loading=false;
                this.getImgRez();
                this.logindec="登陆";
             }else{
                $("#errorTip").text("用户名或者密码不正确");
                this.loading=false;
                this.logindec="登陆";
             }
         })
         .catch(error=>{
            this.loading=false;
            this.logindec="登陆";
         });
       }
     }
  }
}
</script>

<style scoped>
.page{position:absolute;width:100%;height:100%;background:#188AE2;}
.user{width:280px;margin-top:16px;margin-left:60px;}
.loginbox{width:400px;height:280px;margin-left:auto;margin-right:auto;margin-top:300px;border-radius: 2px;background: #fff;box-shadow: 0 0 0 8px rgba(255,255,255,.5);}
.loginbox h6{color:#188AE2;font-size:22px;text-align:center;line-height:38px;padding-top:20px;}
.inputbox{position:relative;}
.labeltip{position:absolute;font-size:12px;color:#ff0000;top:28px;right:10px;}
.error{right:70px;transition:0.5s right;}
.errortip{font-size:12px;color:#ff0000;margin-top:4px;text-indent:60px;}
.login-btn{width:280px;margin-left:60px;margin-top:16px;}
</style>
