<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
     <el-form-item label="科室：" class="flex-100">
        <span class="itemval" v-text="dataInfo.clinicName"></span>
      </el-form-item>
      <el-form-item label="上传资格证" class="flex-100"  style="margin-top:18px;">
          <el-upload
            action="https://jsonplaceholder.typicode.com/posts/"
            class="avatar-uploader"
            :show-file-list="false"
            :before-upload="beforeupload"
            :http-request="uploadSectionFile"
            accept="image/*"
            >
            <img v-if="imageUrl" :src="imageUrl" class="avatar">
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
      </el-form-item>
      <el-form-item label="上传执业证" class="flex-100"  style="margin-top:18px;">
          <el-upload
            action="https://jsonplaceholder.typicode.com/posts/"
            class="avatar-uploader"
            :show-file-list="false"
            :before-upload="beforeupload"
            :http-request="uploadSectionFilea"
            accept="image/*"
            >
            <img v-if="imageUrla" :src="imageUrla" class="avatar">
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button type="primary" @click="update()"
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
      imageUrl:'',
      imageUrla:'',
      imgData:{
        serviceType:'',
        mediaType:''
      },
      workpath:'',
      qualitypath:'',
      applyType:'',
      worktypeDetail:'',
      quailtytypeDetail:'',
      documentsUse:'',
      roleType:[],
      rules: {//校验规则
        clinicID: [required],
        loginName: [required],
      }
    };
  },
  methods: {
    update() {
      if(this.imageUrl!=''&&this.imageUrla!=''){
        this.updateDataInfo(this.$api.doctorclinicAdd,{id:this.id,applyType:this.applyType,clinicID:this.dataInfo.clinicID,id:this.dataInfo.id,certificateofpractice:this.workpath,qualificationcertificate:this.qualitypath},"list");
      }else{
        this.errorMessage("上传证书不能为空");
      }
    },
    uploadSectionFile(content) {//资格证上传
      let $file=this.uploadName()+'.'+content.file.name.split(".").pop();
      let $key=this.uploadData.fileFolder+'/'+$file;
      this.qualitypath=$key;
      this.uploadImage($key,content.file,'quality');
    },
    uploadSectionFilea(content) {//执业证上传
      let $file=this.uploadName()+'.'+content.file.name.split(".").pop();
      let $key=this.uploadData.fileFolder+'/'+$file;
      this.workpath=$key;
      this.uploadImage($key,content.file,'work');
    },
    putService(key,type){//上传图片信息提交
      var $typeDetail;
      if(type=="work"){
        $typeDetail=this.worktypeDetail;
      }else if(type="quality"){
        $typeDetail=this.quailtytypeDetail;
      }
      let obj={
        id: 0,
        documentsUse:this.documentsUse,
        fileType:this.imgData.serviceType,
        typeDetail:$typeDetail,
        address: key,
      };
      this.$http.post(this.$api.clinicPutImg,obj).then(res=>{
        if(res.result.code==200){
          if(type=="work"){
            this.imageUrla=res.result.item.address;
          }else if(type="quality"){
            this.imageUrl=res.result.item.address;
          }
          this.successMessage("图片上传成功");
        }
      });
    },
    beforeupload(file){//上传之前
      //console.log(file);
    },
    getType(){//获取上传类型
      this.$http.post(this.$api.getType).then(res=>{
        this.imgData.mediaType=res.result.item.mediaType[0].code;
        this.imgData.serviceType=res.result.item.type[8].code;
        this.worktypeDetail = res.result.item.typeDetail[2].code;
        this.quailtytypeDetail = res.result.item.typeDetail[3].code;
        this.documentsUse = res.result.item.documentDetail[0].code;
        this.getuploadParams(this.imgData);
      });
    },
    getuploadParams(params){//获取上传参数
      this.$http.post(this.$api.getUpload,params).then(res=>{
          this.uploadData=res.result.item;//图片
      });
    },
    uploadImage(key,file,type){//上传图片
      var $this=this;
      var COS = require('cos-js-sdk-v5');
      var cos = new COS({
        SecretId: this.uploadData.secretId,
        SecretKey: this.uploadData.secretKey,
      });
      if (!file) return;
      cos.putObject({
          Bucket: this.uploadData.bucket, /* 必须 */
          Region: this.uploadData.region,    /* 必须 */
          Key: key,              /* 必须 */
          Body: file, // 上传文件对象
          onProgress: function(progressData) {
              console.log('上传进度', progressData.percent*100)
          }
      }, function(err, data) {
          if(data.statusCode==200){
            $this.putService(key,type);
          } 
      })
    }
  },
  created(){
    this.getType();
    if(this.type=='update'){
      axios
        .all([
          axios.post(this.$api.clinicAllList), //所有科室
          axios.post(this.$api.qualityParam,{Type: 'ConfigPar',SystemCode: 'DocumentsUse'}), //参数
          axios.post(this.$api.doctorclinicData,{id:this.$route.query.id}), //默认数据 
        ])
        .then(
          axios.spread((clinic,param,data) => {
            this.roleType=clinic.result.item;
            this.applyType=param.result.item[2].code;
            this.dataInfo=data.result.item;
            this.imageUrl=this.dataInfo.qualificationcertificate;
            this.imageUrla=this.dataInfo.certificateofpractice;
            this.workpath=this.dataInfo.certificateofpractice.split(".myqcloud.com/")[1];
            this.qualitypath=this.dataInfo.qualificationcertificate.split(".myqcloud.com/")[1];
            this.loading=false;
          })
        )
        .catch(err => {
          this.errorMessage('获取参数失败');
          this.loading = false;
        });
    }else{
      this.dataInfo.clinicID=this.type.split(",")[0];
      this.dataInfo.clinicName=this.type.split(",")[1];
      axios
        .all([
          axios.post(this.$api.clinicAllList), //所有科室
          axios.post(this.$api.qualityParam,{Type: 'ConfigPar',SystemCode: 'DocumentsUse'}), //参数
        ])
        .then(
          axios.spread((clinic,param) => {
            this.roleType=clinic.result.item;
            this.applyType=param.result.item[2].code;
            this.loading=false;
          })
        )
        .catch(err => {
          this.errorMessage('获取参数失败');
          this.loading = false;
        });
    }
  }
};
</script>
<style scoped>
  .avatar-uploader>.el-upload {
    border: 1px dashed #d9d9d9;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
  }
  .avatar-uploader .el-upload:hover {
    border-color: #409EFF;
  }
  .avatar-uploader-icon {
    border: 1px dashed #d9d9d9;
    font-size: 28px;
    color: #8c939d;
    width: 228px;
    height: 158px;
    line-height: 158px;
    text-align: center;
  }
  .avatar {
    width: 228px;
    height: 158px;
    display: block;
    border:1px solid #e5e5e5;
  }
</style>