<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="轮播图名称" class="flex-100" prop="bannerName">
          <el-input v-model="dataInfo.bannerName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="轮播图上传" class="flex-100"  style="margin-top:18px;" prop="bannerImageUrl">
          <el-upload
            action="https://jsonplaceholder.typicode.com/posts/"
            class="avatar-uploader"
            :show-file-list="false"
            :before-upload="beforeupload"
            :http-request="uploadSectionFile"
            accept="image/*"
            >
            <img v-if="dataInfo.bannerImageUrl" :src="dataInfo.bannerImageUrl" class="avatar">
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
      </el-form-item>
      <el-form-item label="类型名称" class="flex-100" prop="bannerTypeCode">
          <el-select @change="getName" placeholder="请选择类型名称" v-model="dataInfo.bannerTypeCode">
              <el-option v-for="(item,key) in codeType" :key="key" :value="item.code" :label="item.name"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="链接地址" class="flex-100">
          <el-input v-model="dataInfo.bannerUrl" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="播放时间段" class="flex-100">
          <el-date-picker
              v-model="dateArr"
              value-format="yyyy-MM-dd"
              :editable='false'
              @change="rangeTime"
              type="daterange"
              range-separator="至"
              start-placeholder="开始日期"
              end-placeholder="结束日期">
          </el-date-picker>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" v-if="isType('update')" @click="update()"
           >确定</el-button>
    </div>           
</section>
</template>
<script>
import axios from "axios";
import va from "@/rules";
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns], 
  data() {
    let required = va.required();
    let number = va.number();
    return {
      dateArr:[],
      codeType:[],
      imgData:{
        serviceType:'',
        mediaType:''
      },
      dataInfo:{
        bannerImageUrl:''
      },
      rules: {//校验规则
        bannerName: [required],
        bannerImageUrl: [required],
        bannerTypeCode: [required]
      },
      paramStatus:{
        type:'ConfigPar',
        systemCode:'BannerType'
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.swiperUpdate,this.dataInfo,"list");
    },
    uploadSectionFile(content) {//图片上传
      let $file=this.uploadName()+'.'+content.file.name.split(".").pop();
      let $key=this.uploadData.fileFolder+'/'+$file;
      this.uploadImage($key,content.file);
    },
    beforeupload(file){//上传之前
      //console.log(file);
    },
    getType(){//获取上传类型
      this.$http.post(this.$api.getType).then(res=>{
        this.imgData.mediaType=res.result.item.mediaType[0].code;
        this.imgData.serviceType=res.result.item.type[4].code;
        this.getuploadParams(this.imgData);
      });
    },
    getuploadParams(params){//获取上传参数
      this.$http.post(this.$api.getUpload,params).then(res=>{
          this.uploadData=res.result.item;//图片
      });
    },
    uploadImage(key,file){//上传图片
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
            $this.dataInfo.bannerImageUrl='http://' + $this.uploadData.bucket + '.cos.' + $this.uploadData.region + '.myqcloud.com' + '/'+key;
            $this.successMessage("图片上传成功");
          } 
      })
    },
    getName(val){
      const obj=this.codeType;
      for(var i=0;i<obj.length;i++){
        if(obj[i].code==val){
           this.dataInfo.bannerTypeName=obj[i].name;
        }
      };
    },
    rangeTime(val){//格式化时间
      if(val!=null){
           this.dataInfo.playStartTime=val[0];
           this.dataInfo.playEndTime=val[1]; 
            if(val[0]===val[1]){
                this.time=val[0];   
            }else{
                this.time=val[0]+'-'+val[1];   
            }    
      }else{
        this.dataInfo.playStartTime='';
        this.dataInfo.playEndTime=''; 
        this.time=this.date.year+'-'+this.date.month+'-'+this.date.day  
      }       
    }
  },
  created(){
    this.getType();
    axios
        .all([
          axios.post(this.$api.baseParams,this.paramStatus),
          axios.post(this.$api.swiperData,{id:this.$route.query.id}), //默认数据 
        ])
        .then(
          axios.spread((base,data) => {
            this.codeType=base.result.item;
            this.dataInfo=data.result.item;
            this.dateArr.push(this.dataInfo.playStartTime);
            this.dateArr.push(this.dataInfo.playEndTime);
            this.loading=false;
          })
        )
        .catch(err => {
          this.errorMessage('获取数据失败');
          this.loading = false;
        });
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
    width: 178px;
    height: 178px;
    line-height: 178px;
    text-align: center;
  }
  .avatar {
    width: 178px;
    height: 178px;
    display: block;
  }
</style>