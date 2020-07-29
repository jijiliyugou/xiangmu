<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
     <el-form-item label="认证类型" class="flex-100" prop="authType">
        <el-select v-model="dataInfo.authType" placeholder="请选择认证类型">
             <el-option v-for="(item,key) in roleType" :key="key" :value="item.code" :label="item.value"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="身份证正面" class="flex-50"  style="margin-top:18px;">
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
      <el-form-item label="身份证反面" class="flex-50"  style="margin-top:18px;">
          <el-upload
            action="https://jsonplaceholder.typicode.com/posts/"
            class="avatar-uploader"
            :show-file-list="false"
            :before-upload="beforeupload"
            :http-request="uploadSectionFileb"
            accept="image/*"
            >
            <img v-if="imageUrlb" :src="imageUrlb" class="avatar">
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
      </el-form-item>
      <el-form-item label="上传资格证" class="flex-50"  style="margin-top:18px;">
          <el-upload
            action="https://jsonplaceholder.typicode.com/posts/"
            class="avatar-uploader"
            :show-file-list="false"
            :before-upload="beforeupload"
            :http-request="uploadSectionFilec"
            accept="image/*"
            >
            <img v-if="imageUrlc" :src="imageUrlc" class="avatar">
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
      </el-form-item>
      <el-form-item label="上传执业证" class="flex-50"  style="margin-top:18px;">
          <el-upload
            action="https://jsonplaceholder.typicode.com/posts/"
            class="avatar-uploader"
            :show-file-list="false"
            :before-upload="beforeupload"
            :http-request="uploadSectionFiled"
            accept="image/*"
            >
            <img v-if="imageUrld" :src="imageUrld" class="avatar">
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
    return {
      imageUrla:'',
      imageUrlb:'',
      imageUrlc:'',
      imageUrld:'',
      imgData:{
        serviceType:'',
        mediaType:''
      },
      //applyType:'',
      identitya:'',
      identityb:'',
      worktypeDetail:'',
      quailtytypeDetail:'',
      documentsUse:'',
      doctorFileApplies:[],
      identityaObj:{},
      identitybObj:{},
      workObj:{},
      qualityObj:{},
      roleType:[],
      rules: {//校验规则
        authType: [required]
      }
    };
  },
  methods: {
    update() {
      if(this.imageUrla!=''&&this.imageUrlb!=''&&this.imageUrlc!=''&&this.imageUrld!=''){
        let arrObj=[this.identityaObj,this.identitybObj,this.qualityObj,this.workObj];
        this.updateDataInfo(this.$api.identityAdd,{doctorFileApplies:arrObj,authType:this.dataInfo.authType,AuthCheckRes:"upload"},"list");
      }else{
        this.errorMessage("上传证书不能为空");
      }
    },
    uploadSectionFilea(content) {//身份证正面上传
      let $file=this.uploadName()+'.'+content.file.name.split(".").pop();
      let $key=this.uploadData.fileFolder+'/'+$file;
      this.uploadImage($key,content.file,'identitya');
    },
    uploadSectionFileb(content) {//身份证反面上传
      let $file=this.uploadName()+'.'+content.file.name.split(".").pop();
      let $key=this.uploadData.fileFolder+'/'+$file;
      this.uploadImage($key,content.file,'identityb');
    },
    uploadSectionFilec(content) {//资格证上传
      let $file=this.uploadName()+'.'+content.file.name.split(".").pop();
      let $key=this.uploadData.fileFolder+'/'+$file;
      console.log($key);
      this.uploadImage($key,content.file,'quality');
    },
    uploadSectionFiled(content) {//执业证上传
      let $file=this.uploadName()+'.'+content.file.name.split(".").pop();
      let $key=this.uploadData.fileFolder+'/'+$file;
      this.uploadImage($key,content.file,'work');
    },
    putService(key,type){//上传图片信息对象
      if(type=="work"){
        this.imageUrld='http://' + this.uploadData.bucket + '.cos.' + this.uploadData.region + '.myqcloud.com' + '/'+key;
        this.workObj={
          id: 0,
          documentsUse:this.documentsUse,
          fileType:this.imgData.serviceType,
          typeDetail:this.worktypeDetail,
          address: this.imageUrld,
        }
      }else if(type=="quality"){
        this.imageUrlc='http://' + this.uploadData.bucket + '.cos.' + this.uploadData.region + '.myqcloud.com' + '/'+key;
        this.qualityObj={
          id: 0,
          documentsUse:this.documentsUse,
          fileType:this.imgData.serviceType,
          typeDetail:this.quailtytypeDetail,
          address: this.imageUrlc,
        }
      }else if(type=="identitya"){
        this.imageUrla='http://' + this.uploadData.bucket + '.cos.' + this.uploadData.region + '.myqcloud.com' + '/'+key;
        this.identityaObj={
          id: 0,
          documentsUse:this.documentsUse,
          fileType:this.imgData.serviceType,
          typeDetail:this.identitya,
          address: this.imageUrla,
        }
      }else if(type=="identityb"){
        this.imageUrlb='http://' + this.uploadData.bucket + '.cos.' + this.uploadData.region + '.myqcloud.com' + '/'+key;
        this.identitybObj={
          id: 0,
          documentsUse:this.documentsUse,
          fileType:this.imgData.serviceType,
          typeDetail:this.identityb,
          address: this.imageUrlb,
        }
      }
    },
    beforeupload(file){//上传之前
      //console.log(file);
    },
    getType(){//获取上传类型
      this.$http.post(this.$api.getType).then(res=>{
        this.imgData.mediaType=res.result.item.mediaType[0].code;
        this.imgData.serviceType=res.result.item.type[8].code;
        this.identitya = res.result.item.typeDetail[0].code;//身份证正面
        this.identityb = res.result.item.typeDetail[1].code;//身份证反面
        this.worktypeDetail = res.result.item.typeDetail[2].code;//资格证
        this.quailtytypeDetail = res.result.item.typeDetail[3].code;//执业证
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
              //console.log('上传进度', progressData.percent*100)
          }
      }, function(err, data) {
          if(data.statusCode==200){
            $this.putService(key,type);
          } 
      })
    }
  },
  created(){
    this.loading=false;
    this.getType();
    this.$http.post(this.$api.identityType).then(res=>{
      this.roleType=res.result.item;
    }).catch(error=>{this.errorMessage('获取基础参数失败');});
    this.$http.post(this.$api.identityData).then(res=>{//数据回填
      if(res.result.code==200){
        const resultArr=res.result.item.doctorFileApplies;
        this.dataInfo=res.result.item;
        for(var i=0;i<resultArr.length;i++){
          if(resultArr[i].typeDetail=='idcardup'){//身份证正面
            this.imageUrla=resultArr[i].address;
            this.identityaObj={
              id: 0,
              documentsUse:resultArr[i].documentsUse,
              fileType:resultArr[i].fileType,
              typeDetail:resultArr[i].typeDetail,
              address: resultArr[i].address,
            }
          }else if(resultArr[i].typeDetail=='idcarddown'){//身份证反面
            this.imageUrlb=resultArr[i].address;
            this.identitybObj={
              id: 0,
              documentsUse:resultArr[i].documentsUse,
              fileType:resultArr[i].fileType,
              typeDetail:resultArr[i].typeDetail,
              address: resultArr[i].address,
            }
          }else if(resultArr[i].typeDetail=='certificateofpractice'){//执业证
            this.imageUrld=resultArr[i].address;
            this.workObj={
              id: 0,
              documentsUse:resultArr[i].documentsUse,
              fileType:resultArr[i].fileType,
              typeDetail:resultArr[i].typeDetail,
              address: resultArr[i].address,
            }
          }else if(resultArr[i].typeDetail=='qualificationcertificate'){//资格证
            this.imageUrlc=resultArr[i].address;
            this.qualityObj={
              id: 0,
              documentsUse:resultArr[i].documentsUse,
              fileType:resultArr[i].fileType,
              typeDetail:resultArr[i].typeDetail,
              address: resultArr[i].address,
            }
          }
        }
      }
    }).catch(error=>{this.errorMessage('获取数据失败');});
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
    border: 1px solid #e5e5e5;
  }
</style>