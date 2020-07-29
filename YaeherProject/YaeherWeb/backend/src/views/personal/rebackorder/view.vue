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
      <el-form-item label="病情描述：" class="flex-100">
          <span class="itemval" v-text="dataInfo.iiInessDescription"></span>
          <div v-for="(item,key) in dataInfo.consultationfile">
            <div v-if="item.mediatype=='image'" class="imgbox"><img :src="item.message"><div class="ivshage" @click="viewOpen(item.message,'image')"><i class="el-icon-zoom-in"></i></div></div>
            <div v-else-if="item.mediatype=='video'" class="imgbox"><div class="playbox" @click="viewOpen(item.message,'video')"></div></div>
          </div>
      </el-form-item>
      <el-form-item label="咨询状态：" class="flex-100">
          <span class="itemval" v-text="dataInfo.consultState"></span>
      </el-form-item>
      <template v-if="dataInfo.consultState=='已退单'">
        <el-form-item label="退单原因描述：" class="flex-100">
            <span class="itemval" v-text="dataInfo.refundRemarks"></span>
        </el-form-item>
        <el-form-item label="推荐医生：" class="flex-100" v-if="dataInfo.recommendDoctorID!=0">
            <div slot-scope="scope" class="imagebox">
               <img :src="dataInfo.recommendDoctorImage" class="userimages"><span class="itemval" v-text="dataInfo.recommendDoctorName"></span>
            </div>
        </el-form-item>
      </template>
      <el-form-item class="flex-100" v-for="(item,key) in dataInfo.replys" :key="key" :label="item.replyType=='answer'?'回复：':'追问：'">
          <span class="itemval" v-if="item.message!=''" v-text="item.message"></span>
          <span class="itemval" v-if="item.answerType=='Phone'" v-text="'您在 '+formatTime(item.createdOn)+' 进行了电话回复'"></span>
          <div v-for="(item,key) in item.consultationFile">
            <div v-if="item.mediaType=='image'" class="imgbox"><img :src="item.fileUrl"><div class="ivshage" @click="viewOpen(item.fileUrl,'image')"><i class="el-icon-zoom-in"></i></div></div>
            <div v-else-if="item.mediaType=='video'" class="imgbox"><div class="playbox" @click="viewOpen(item.fileUrl,'video')"></div></div>
          </div>
      </el-form-item>
    </el-form>
    <el-form  ref="form" :model="appriseInfo"
        class="yh-form" label-width="150px">
      <el-form-item label="退单原因：" class="flex-50">
          <span class="itemval" v-text="appriseInfo.refundReason==null?'无':JSON.parse(appriseInfo.refundReason).LabelName"></span>
      </el-form-item>
      <el-form-item label="退单原因描述：" class="flex-50">
          <span class="itemval" v-text="appriseInfo.refundRemarks"></span>
      </el-form-item>
      <el-form-item label="订单金额：" class="flex-50">
          <span class="itemval" v-text="'￥'+appriseInfo.orderMoney"></span>
      </el-form-item>
      <el-form-item label="审核状态：" class="flex-50">
          <span class="itemval" v-text="checkType(appriseInfo.checkState)"></span>
      </el-form-item>
      <el-form-item label="退单时间：" class="flex-50">
          <span class="itemval" v-text="formatTime(appriseInfo.createdOn)"></span>
      </el-form-item>
    </el-form>        
    <div class="form-footer" v-if="appriseInfo.checkState=='checking'">
        <el-button :loading="loading" type="success" @click="pass('success')"
           >通过</el-button>
        <el-button :loading="loading" type="danger" @click="pass('fail')"
           >不通过</el-button>
    </div>
    <!--图片查看视频播放-->
    <el-dialog :visible.sync="ImgVdoDialog" :title="mediaoftype=='image'?'图片查看':'视频播放'">
      <div v-if="mediaoftype=='image'"><img :src="ImgVdoUrl" width="100%"></div>
      <div v-else-if="mediaoftype=='video'"><video :src="ImgVdoUrl" controls width="100%" autoplay></video></div>
    </el-dialog>           
</section>
</template>
<script>
import axios from "axios";
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns],
  data() {
    return {
      ImgVdoDialog:false,
      ImgVdoUrl:'',
      appriseID:'',
      consultID:'',
      mediaoftype:'',
      dataInfo:{},
      appriseInfo:{}
    }
  },
  methods:{
    viewOpen(url,type){
      this.mediaoftype=type;
      this.ImgVdoDialog=true;
      this.ImgVdoUrl=url;
    },
    typeCheck(type){
      if(type=='Phone'){return '电话'}else if(type=='ImageText'){return '图文'}
    },
    typeState(type){
      if(type=='back'){return '已退回'}else if(type=='treated'){return '已处理'}else if(type=='untreated'){return '未处理'}
    },
    checkType(val){
      if(val=='checking'){
        return '审核中';
      }else if(val=='success'){
        return '已通过';
      }else if(val=='fail'){
        return '不通过';
      }
    },
    pass(state){
      if(state=='success'){
        this.$http.post(this.$api.rebackupdate,{id:this.appriseID,checkState:state}).then(res=>{
          if(res.result.code==200){
            this.successMessage('操作成功');
            this.routerLink('list');
          }
        }).catch(error=>{this.errorMessage('操作失败');});
      }else if(state=='fail'){
        this.$http.post(this.$api.rebackupdate,{id:this.appriseID,checkState:state}).then(res=>{
          if(res.result.code==200){
            this.successMessage('操作成功');
            this.routerLink('list');
          }
        }).catch(error=>{this.errorMessage('操作失败');});
      }
    }
  },
  created(){
    this.appriseID=this.id.split(",")[0];
    this.consultID=this.id.split(",")[1];
    axios.all([
        axios.post(this.$api.referDetail,{id:this.consultID}),
        axios.post(this.$api.retreatDetail,{id:this.appriseID})
      ]).then(axios.spread((consult,apprise)=>{
        this.dataInfo=consult.result.item;
        this.appriseInfo=apprise.result.item;
        this.loading=false;
      })) .catch(error => {
        this.loading = false;
        this.errorMessage("获取数据失败");
      });
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.userimages{float: left;}
</style>