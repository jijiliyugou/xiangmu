<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <!-- <el-form-item label="头像" class="flex-100">
        <yh-upload></yh-upload>
      </el-form-item> -->
      <!-- <el-form-item label="上传头像" class="flex-100" prop="userImage">
          <el-input type="file" v-model="dataInfo.fullName" class="yh-width-fixed"></el-input>
      </el-form-item> -->
      <el-form-item label="姓名" class="flex-100" prop="fullName">
          <el-input v-model="dataInfo.fullName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="系统登录名" class="flex-100" prop="loginName">
          <el-input v-model="dataInfo.loginName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="性别" class="flex-100">
        <template>
          <el-radio-group v-model="dataInfo.sex">
            <el-radio :label="1">男</el-radio>
            <el-radio :label="2">女</el-radio>
          </el-radio-group>
        </template>
      </el-form-item>
      <el-form-item label="生日" class="flex-100" prop="birthday">
          <el-date-picker
            v-model="dataInfo.birthday"
            type="date"
            value-format="yyyy-MM-dd"
            placeholder="选择日期">
          </el-date-picker>
      </el-form-item>
      <el-form-item label="电话" class="flex-100" prop="phoneNumber">
          <el-input v-model="dataInfo.phoneNumber" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="邮箱" class="flex-100">
          <el-input v-model="dataInfo.email" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="用户来源" class="flex-100" prop="userorigin">
          <el-input v-model="dataInfo.userorigin" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="关联微信" class="flex-100" prop="wecharNo">
          <el-input v-model="dataInfo.wecharNo" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="平台角色" class="flex-100" prop="roleName">
        <el-select v-model="dataInfo.roleName" placeholder="请选择平台角色">
             <el-option v-for="(item,key) in roleType" :key="key" :value="item.code" :label="item.name"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="用户密码" class="flex-100" prop="loginPwd">
          <el-input type="password" v-model="dataInfo.loginPwd" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="是否激活" class="flex-100">
        <template>
          <el-radio-group v-model="dataInfo.enabled">
            <el-radio :label="true">是</el-radio>
            <el-radio :label="false">否</el-radio>
          </el-radio-group>
        </template>
      </el-form-item>
      
    </el-form>
    <div class="form-footer">
        <el-button type="primary" v-if="isType('add')" @click="update()"
           >确定</el-button>
    </div>           
</section>
</template>
<script>
import axios from "axios";
import va from "@/rules";
import formMixns from "@/mixins/form";
import md5 from 'js-md5';
export default {
  mixins: [formMixns], 
  data() {
    let required = va.required();
    let number = va.number();
    let email = va.email();
    let mobile = va.mobile();
    return {
      imgData:{
        serviceType:'',
        mediaType:''
      },
      roleType:[],
      dataInfo:{
        enabled:true,
        sex:1
      },
      rules: {//校验规则
        fullName: [required],
        loginName: [required],
        phoneNumber: [required,mobile],
        //email: [required,email],
        sex: [required],
        birthday: [required],
        userorigin: [required],
        wecharNo: [required],
        loginPwd:[required],
        roleName: [required]
      }
    };
  },
  methods: {
    update() {
      if(this.dataInfo.loginPwd!=undefined){
        this.dataInfo.loginPwd=md5(this.dataInfo.loginPwd);
      }
      this.saveDataInfo(this.$api.userAdd,this.dataInfo,"list");
    }
  },
  created(){
    this.loading=false;
    this.$http.post(this.$api.userRoleList).then(res=>{
      this.loading=false;
      this.roleType=res.result.item;
    }).catch(error=>{this.errorMessage('获取基础参数失败');});
    this.$http.post(this.$api.getType).then(res=>{
      this.imgData.mediaType=res.result.item.mediaType[0].code;
      this.imgData.serviceType=res.result.item.type[3].code;
      this.$http.post(this.$api.getUpload,this.imgData).then(res=>{
    });
    });
  }
};
</script>

<style scoped>
</style>