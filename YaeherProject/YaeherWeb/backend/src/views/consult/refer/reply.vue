<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back> 
    <el-form  ref="form" :model="dataInfo"
        class="yh-form" label-width="150px">
      <el-form-item label="咨询单号：" class="flex-50">
          <span class="itemval" v-text="dataInfo.consultNumber"></span>
      </el-form-item>
      <el-form-item label="咨询姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.consultantName"></span>
      </el-form-item>
      <el-form-item label="患者姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.patientName"></span>
      </el-form-item>
      <el-form-item label="患者性别：" class="flex-50">
          <div slot-scope="scope">
             <label v-if="dataInfo.sex==1">男</label><label v-else>女</label>
          </div>
      </el-form-item>
      <el-form-item label="患者年龄：" class="flex-50">
          <span class="itemval" v-text="dataInfo.age"></span>
      </el-form-item>
      <el-form-item label="医生姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.doctorName"></span>
      </el-form-item>
      <el-form-item label="咨询类型：" class="flex-50">
          <span class="itemval" v-text="typeCheck(dataInfo.consultType)"></span>
      </el-form-item>
      <el-form-item label="药物过敏：" class="flex-50">
          <span class="itemval" v-text="dataInfo.allergicHistory==''?'无':dataInfo.allergicHistory"></span>
      </el-form-item>
      <el-form-item label="疾病类型：" class="flex-50">
          <span class="itemval" v-text="dataInfo.iiInessType"></span>
      </el-form-item>
      <el-form-item label="所在城市：" class="flex-50">
          <span class="itemval" v-text="dataInfo.patientCity"></span>
      </el-form-item>
      <el-form-item v-if="dataInfo.consultType!='Phone'" label="剩余追问次数：" class="flex-50">
          <span class="itemval" v-text="dataInfo.hasInquiryTimes"></span>
      </el-form-item>
      <el-form-item label="咨询状态：" class="flex-50">
          <span class="itemval" v-text="dataInfo.consultState"></span>
      </el-form-item>
      <el-form-item label="咨询时间：" class="flex-100">
          <span class="itemval" v-text="formatTime(dataInfo.createdOn)"></span>
      </el-form-item>
      <el-form-item label="病情描述：" class="flex-100">
          <span class="itemval" v-text="dataInfo.iiInessDescription"></span>
          <viewer :images="dataInfo.consultationfile">
            <div v-for="(item,key) in dataInfo.consultationfile">
              <div v-if="item.mediatype=='image'" class="imgbox"><img :data-source="item.message" :src="item.message.replace('cos.ap-guangzhou','picgz')+imgRez"></div>
              <div v-else-if="item.mediatype=='video'" class="imgbox"><div class="playbox" @click="viewOpen(item.message,'video')"></div></div>
            </div>
          </viewer>
      </el-form-item>
      <el-form-item class="flex-100" v-for="(item,key) in dataInfo.replys" :key="key" :label="item.replyType=='answer'?'回复：':'追问：'" style="margin-top:8px;">
          <span class="itemval" v-if="item.message!=''" v-html="item.message"></span>
          <span class="itemval" v-if="item.answerType=='Phone'" v-text="'您在 '+formatTime(item.createdOn)+' 进行了电话回复'"></span>
          <viewer :images="item.consultationFile">
            <div v-for="(item,key) in item.consultationFile">
              <div v-if="item.mediaType=='image'" class="imgbox"><img :data-source="item.fileUrl" :src="item.fileUrl.replace('cos.ap-guangzhou','picgz')+imgRez"></div>
              <div v-else-if="item.mediaType=='video'" class="imgbox"><div class="playbox" @click="viewOpen(item.fileUrl,'video')"></div></div>
              <div v-else-if="item.mediaType=='voice'" class="voice"><audio :src="item.fileUrl" controls="controls">亲 您的浏览器不支持html5的audio标签</audio></div>
            </div>
          </viewer>
          <p class="timeStyle">{{formatTime(item.createdOn)}}</p>
      </el-form-item>
      <el-form-item label="回复内容" class="flex-100" style="margin-top:18px;">
        <div class="reasonbox"> 
          <el-input type="textarea" :rows="8" placeholder="请输入回复内容" v-model="paramData.repayIllnessDescription" class="yh-width-fixed" @keyup.native="keyon" @change="key"></el-input>
          <label class="lettertype">{{letterNum}}/{{maxLength}}</label>
        </div>
      </el-form-item>
      <el-form-item label="快捷回复" class="flex-100" style="margin-top:18px;">
        <el-select placeholder="请选择快捷回复" v-model="fastreply" @change="query">
             <el-option v-for="(item,key) in fastData" :key="key" :label="item.content" :value="item.content"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="上传图片" class="flex-100"  style="margin-top:18px;">
          <el-upload
            list-type="picture-card"
            :before-upload="beforeupload"
            :on-preview="handlePictureCardPreview"
            :http-request="uploadSectionFile"
            ref="imgupload"
            :on-change="fileChange"
            :before-remove="preRemove"
            :limit="9"
            :on-exceed="limitNum"
            accept="image/*"
            action=""
            >
            <i class="el-icon-plus"></i>
          </el-upload>
          <el-dialog :visible.sync="dialogVisible" :title="'图片预览'">
            <img width="100%" :src="dialogImageUrl" alt="">
          </el-dialog>
      </el-form-item>
      <!-- <el-form-item label="上传视频" class="flex-100" style="margin-top:18px;">
          <el-upload
            :file-list="fileVList"
            :before-upload="beforeuploada"
            :on-preview="handlePictureCardPreviewa"
            :http-request="uploadSectionFile"
            ref="vdoupload"
            :on-change="fileChangea"
            :before-remove="preRemovea"
            accept="video/*"
            action=""
            >
            <el-button type="primary">点击上传</el-button>
          </el-upload>
      </el-form-item> -->
    </el-form>       
    <div class="form-footer">
        <!-- <el-button type="success"  @click="$router.go(-1)"
           >返回</el-button> -->
        <el-button type="primary" v-if="isType('update')" @click="update()"
       >确定</el-button><el-button v-if="dataInfo.consultType=='Phone'&&dataInfo.consultState!='已完成'&&dataInfo.consultState!='已退单'" type="primary" @click="tipOpen()"
       >拨打电话</el-button>
       <el-button v-if="dataInfo.canhargeback==true" type="success"  @click.native="routerLink('/refer/reback','reback',$route.query.id)">退单</el-button>
    </div> 
    <!-- 修改手机号码 -->
    <el-dialog
      class="shadebox"
      :title="'修改手机号码'"
      :visible.sync="mobileDialogVisible"
      width="380px"
      height="400px"
      center>
      <el-form ref="form" label-width="100px">
        <el-form-item label="手机号码：" prop="mobileNum">
          <el-input v-model="mobileNum"></el-input>
        </el-form-item>
        <el-form-item label="验证码：" prop="workYear">
          <el-input v-model="code" style="width:120px;"></el-input><el-button type="primary" @click="codeApply()" style="float:right;width:100px;" :disabled="codeStatus">{{mobliebtn}}</el-button>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="mobileApply()">确定</el-button>
      </span>
    </el-dialog>    
    <!--图片查看视频播放-->
      <el-dialog :visible.sync="ImgVdoDialog" :title="mediaoftype=='image'?'图片查看':'视频播放'">
        <div v-if="mediaoftype=='image'"><img :src="ImgVdoUrl" width="100%"></div>
        <div v-else-if="mediaoftype=='video'"><video :src="ImgVdoUrl" controls width="100%" autoplay></video></div>
      </el-dialog>     
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
    return {
      imgRez:localStorage.getItem('imRez')==undefined?'':localStorage.getItem('imRez'),
      imgSize:0,
      fastreply:'',
      fastData:[],
      timer:null,
      cacheContent:'',
      maxLength:5000,
      letterNum:0,
      ImgVdoDialog:false,
      ImgVdoUrl:'',
      mediaoftype:'',
      fileVList:[],
      imgList:[],
      vdoList:[],
      fileList:[],
      number:90,
      mobileNum:'',
      code:'',
      codeStatus:false,
      mobliebtn:'发送验证码',
      mobileDialogVisible:false,//手机号码
      caller:'',
      called:'',
      dialogImageUrl: '',
      dialogVisible: false,
      paramData:{repayIllnessDescription:'',consultID:this.$route.query.id,userID:localStorage.getItem('userId'),ReplyTypeCode:'answer',ReplyStateCode:'submission'},
      rules: {//校验规则
        repayIllnessDescription: [required]
      },
      dataInfo:{
      },
      imgData:{
        serviceType:'',
        mediaType:''
      },
      vdoData:{
        serviceType:'',
        mediaType:''
      },
      uploadData:{},
      uploadVData:{}
    }
  },
  methods: {
    query(){
      this.paramData.repayIllnessDescription=this.paramData.repayIllnessDescription+this.fastreply;
      this.letterNum=this.paramData.repayIllnessDescription.length;
      if(this.letterNum>this.maxLength){
        this.paramData.repayIllnessDescription=this.paramData.repayIllnessDescription.substring(0,this.maxLength);
        this.letterNum=this.paramData.repayIllnessDescription.length;
        this.errorMessage("回复内容已超过5000字，系统已自动为您删除溢出的文本。");
      }
    },
    keyon(){
      if(this.paramData.repayIllnessDescription!=undefined){
        this.letterNum=this.paramData.repayIllnessDescription.length;
        if(this.letterNum>this.maxLength){
          this.paramData.repayIllnessDescription=this.paramData.repayIllnessDescription.substring(0,this.maxLength);
          this.letterNum=this.paramData.repayIllnessDescription.length;
        }
      }
    },
    key(){
      if(this.paramData.repayIllnessDescription!=undefined){
        this.letterNum=this.paramData.repayIllnessDescription.length;
        if(this.letterNum>this.maxLength){
          this.paramData.repayIllnessDescription=this.paramData.repayIllnessDescription.substring(0,this.maxLength);
          this.letterNum=this.paramData.repayIllnessDescription.length;
        }
      }
    },
    viewOpen(url,type){
      this.mediaoftype=type;
      this.ImgVdoDialog=true;
      this.ImgVdoUrl=url;
    },
    fileChange(file, fileList){//文件改变
    },
    limitNum(){
      this.errorMessage("一次最多只能上传9张图片");
    },
    beforeupload(file){//上传之前
      if(file.size>this.imgSize){
        this.errorMessage("上传的图片大小已超过"+this.imgSize/1024000+"M，请重新上传！");
        return false;
      }
    },
    preRemove(file, fileList) {//删除图片之前
      if(fileList.length>0){
        for(let i=0;i<fileList.length;i++){
          if(file==fileList[i]){
            this.imgList.splice(i,1);
          }
        }
      }
    },
    handlePictureCardPreview(file) {
      this.dialogImageUrl = file.url;
      this.dialogVisible = true;
    },
    uploadSectionFile(file) {//自定义上传
      let $file=this.uploadName()+'.'+file.file.name.split(".").pop();
      var $key,$type,obj;
      if(file.file.type.split('/')[0]==this.imgData.mediaType){
        $key=this.uploadData.fileFolder+'/'+$file;
        $type=this.imgData.mediaType;
        obj={
          id: 0,
          filename: $file,
          mediaType: $type,
          fileSize: file.file.size
        };
        this.imgList.push(obj);
      }else if(file.file.type.split('/')[0]==this.vdoData.mediaType){
        $key=this.uploadVData.fileFolder+'/'+$file;
        $type=this.vdoData.mediaType;
        obj={
          id: 0,
          filename: $file,
          mediaType: $type,
          fileSize: file.file.size
        };
        this.vdoList.push(obj);
      }
      this.uploadImage($key,file.file,file);
    },
    fileChangea(file, fileList){//文件改变
    },
    beforeuploada(file){//上传之前
      /*if(file.size>this.imgSize){
        this.errorMessage("上传的视频大小已超过"+this.imgSize/1024000+"M，请重新上传！");
        return false;
      }*/
    },
    preRemovea(file, fileList) {//删除视频之前
      if(fileList.length>0){
        for(let i=0;i<fileList.length;i++){
          if(file==fileList[i]){
            this.vdoList.splice(i,1);
          }
        }
      }
    },
    handlePictureCardPreviewa(file) {
      this.dialogImageUrl = file.url;
      this.dialogVisible = true;
    },
    update() {//提交
      this.paramData.attach=this.imgList.concat(this.vdoList);
      if($(".el-upload-list .is-success").length<this.paramData.attach.length){
        this.errorMessage("请等待图片视频全部上传完毕之后再进行回复！");
        return false;
      }
      if(this.paramData.repayIllnessDescription.trim()==""&&this.paramData.attach.length==0){
        this.errorMessage("回复内容不能为空！");
        return false;
      }
      this.paramData.repayIllnessDescription=this.paramData.repayIllnessDescription.replace(/\n|\r/g,'<br/>');
      let form = this.$refs['form'];
      form.validate((valid) => {//验证方法
          if (valid) {
              this.loading = true;
              axios.post(this.$api.referReply,this.paramData)
                  .then(res => {
                      if (res.result.code == "200") {
                          this.successMessage('回复成功');
                          this.imgList=[];
                          this.vdoList=[];
                          this.$refs.imgupload.clearFiles();
                          //this.$refs.vdoupload.clearFiles();
                          this.paramData.repayIllnessDescription="";
                          this.getDataInfo(this.$api.referDetail,{id:this.$route.query.id});
                          this.cacheClear();
                      }
                  }).catch(error => { this.loading = false; })
          } else {
              return false
          }
      })
      //this.handleDataInfo(this.$api.referReply,this.paramData,"list");
    },
    getType(){//获取上传类型
      this.$http.post(this.$api.getType).then(res=>{
        this.imgSize=res.result.item.consultationImagesize*1024000;
        this.imgData.mediaType=res.result.item.mediaType[0].code;
        this.imgData.serviceType=res.result.item.type[2].code;
        this.vdoData.mediaType=res.result.item.mediaType[1].code;
        this.vdoData.serviceType=res.result.item.type[2].code;
        this.getuploadParams(this.imgData,"image");
        this.getuploadParams(this.vdoData,"video");
      });
    },
    getuploadParams(params,type){//获取上传参数
      this.$http.post(this.$api.getUpload,params).then(res=>{
        if(type=="image"){
          this.uploadData=res.result.item;
        }else{
          this.uploadVData=res.result.item;
        }
      });
    },
    uploadImage(key,file,filedata){//上传图片
      let $this=this;
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
            filedata.file.percent = progressData.percent*100;
            filedata.onProgress(filedata.file);
          }
      }, function(err, data) {
          if(err){
            $this.errorMessage(err+"请重新上传图片！");
            return false;
          };
          if(data.statusCode==200){
            filedata.onSuccess();
          } 
      })
    },
    phonecall(){//拨打电话
      this.$http.post(this.$api.phoneReply,{caller:this.dataInfo.doctorPhoneNumber,callee:this.dataInfo.phoneNumber,consultID:this.$route.query.id}).then(res=>{
        if(res.result.code==200){
          this.successMessage("拨打电话成功，请留意您的手机");
        }

      }).catch(error=>{
        //this.errorMessage("拨打电话失败");
      });
      this.successMessage("拨打电话成功，请留意您的手机");
    },
    tipOpen() {
      const tips="请确定您呼叫的手机号码是否为："+this.dataInfo.doctorPhoneNumber+"，拨打电话之前请先确定手机在您的身边，如果不是请先修改您的手机号码！";
      this.$confirm(tips, '提示', {
        confirmButtonText: '是',
        cancelButtonText: '修改号码',
        type: 'warning',
        distinguishCancelAndClose:true,
        center: true
      }).then(() => {
        this.phonecall();
      }).catch((action) => {
        if(action=='cancel'){
          this.mobileDialogVisible=true;
          this.mobileNum=this.dataInfo.doctorPhoneNumber;
        }
      });
    },
    codeApply(){//发送验证码
      let reg = /^1\d{10}$/;
      if (!reg.test(this.mobileNum)) {
          this.errorMessage('请输入正确的手机号');
          return false;
      }
      function settime() {//倒计时
        if (countDown == 0) {
            $this.codeStatus=false;
            $this.mobliebtn="再次获取";
            countDown = num;
            return;
        } else {
            $this.codeStatus=true;
            $this.mobliebtn=countDown + "s";
            countDown--;
            setInterval(function () {
                settime();
            }, 1000)
        }
      }
      const $this=this;
      var num = 90;
      var countDown = num;
      this.$http.post(this.$api.codeSend,{messageType: "Authentication",phoneNumber:this.mobileNum}).then(res=>{
        if(res.result.code==200){
          settime();
        }
      }).catch(error=>{this.errorMessage("发送验证失败")});
    },
    mobileApply(){//修改手机号
      this.$http.post(this.$api.phoneUpdate,{messageType: "Authentication",phoneNumber:this.mobileNum,verificationCode:this.code,id:localStorage.getItem('userId')}).then(res=>{
        if(res.result.code==200){
          this.successMessage("修改成功");
          this.mobileDialogVisible=false;
        }
      }).catch(error=>{this.errorMessage("修改失败")});
    },
    typeCheck(type){
      if(type=='Phone'){return '电话'}else if(type=='ImageText'){return '图文'}
    },
    cachePut(){//缓存提交
      if(this.cacheContent!=this.paramData.repayIllnessDescription){
        this.$http.post(this.$api.cacheUpdate,{consultNumber:this.$route.query.consultNumber,repayIllnessDescription:this.paramData.repayIllnessDescription})
          .then(res => {  
              if (res.result.code == 200) {
                this.cacheContent=this.paramData.repayIllnessDescription;
              }
          }).catch(error => {this.errorMessage("缓存提交失败");});
      }
    },
    cacheClear(){//缓存清除
      this.$http.post(this.$api.cacheDelete,{consultNumber:this.$route.query.consultNumber})
          .then(res => {  
              if (res.result.code == 200) {
                this.cacheContent="";
              }
          }).catch(error => {this.errorMessage("缓存清除失败");});
    },
    cacheGet(){//缓存获取
      this.$http.post(this.$api.cacheData,{consultNumber:this.$route.query.consultNumber})
        .then(res => {  
            if (res.result.code == 200) {
              this.paramData.repayIllnessDescription=res.result.item;
              this.letterNum=this.paramData.repayIllnessDescription.length;
              this.cacheContent=res.result.item;
            }
        }).catch(error => {this.errorMessage("缓存获取失败");});
    }
  },
  destroyed(){
    clearInterval(this.timer);
    this.timer=null;
  },
  created(){
    this.getDataInfo(this.$api.referDetail,{id:this.$route.query.id});
    this.getType();
    this.cacheGet();
    let $this=this;
    this.timer=setInterval(function(){
      $this.cachePut();
    },10000);
    //快捷回复
    this.$http.post(this.$api.fastreplyListnopage,{keyWord:'',startTime:'',endTime:'',skipCount:1,maxResultCount:99})
      .then(res => {  
          if (res.result.code == 200) {
            this.fastData=res.result.item;
          }
      }).catch(error => {});
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.el-select-dropdown__item{max-width: 212px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;}
</style>