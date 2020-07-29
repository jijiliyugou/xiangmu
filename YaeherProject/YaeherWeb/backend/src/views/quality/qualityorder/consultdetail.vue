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
      <el-form-item label="联系电话：" class="flex-50">
          <span class="itemval" v-text="dataInfo.phoneNumber"></span>
      </el-form-item>
      <el-form-item label="所在城市：" class="flex-50">
          <span class="itemval" v-text="dataInfo.patientCity"></span>
      </el-form-item>
      <el-form-item label="剩余追问次数：" class="flex-50">
          <span class="itemval" v-text="dataInfo.hasInquiryTimes"></span>
      </el-form-item>
      <el-form-item label="咨询时间：" class="flex-50">
          <span class="itemval" v-text="formatTime(dataInfo.createdOn)"></span>
      </el-form-item>
      <el-form-item label="病情描述：" class="flex-100">
          <span class="itemval" v-text="dataInfo.iiInessDescription"></span>
          <viewer :images="dataInfo.consultationfile">
            <div v-for="(item,key) in dataInfo.consultationfile">
              <div v-if="item.mediatype=='image'" class="imgbox"><img :src="item.message"></div>
              <div v-else-if="item.mediatype=='video'" class="imgbox"><div class="playbox" @click="viewOpen(item.message,'video')"></div></div>
            </div>
          </viewer>
      </el-form-item>
      <el-form-item label="咨询状态：" class="flex-100">
          <span class="itemval" v-text="dataInfo.consultState"></span>
      </el-form-item>
      <template v-if="dataInfo.consultState=='已退单'">
        <!-- <el-form-item label="退单理由：" class="flex-100">
            <span class="itemval" v-text="JSON.parse(dataInfo.refundReason).LabelName"></span>
        </el-form-item> -->
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
          <viewer :images="item.consultationFile">
            <div v-for="(item,key) in item.consultationFile">
              <div v-if="item.mediaType=='image'" class="imgbox"><img :src="item.fileUrl"></div>
              <div v-else-if="item.mediaType=='video'" class="imgbox"><div class="playbox" @click="viewOpen(item.fileUrl,'video')"></div></div>
              <div v-else-if="item.mediaType=='voice'" class="voice"><audio :src="item.fileUrl" controls="controls">亲 您的浏览器不支持html5的audio标签</audio></div>
            </div>
          </viewer>
          <p class="timeStyle">{{formatTime(item.createdOn)}}</p>
      </el-form-item>
      <el-form-item label="质控评分：" class="flex-100" v-if="dataInfo.qualityLevel!=0">
          <span class="itemval" v-text="dataInfo.qualityLevel"></span>
      </el-form-item>
      <el-form-item label="质控评语：" class="flex-100" v-if="dataInfo.qualityReason!=''">
          <span class="itemval" v-text="dataInfo.qualityReason"></span>
      </el-form-item>
      <template v-if="dataInfo.qualityControlManage!=null" v-for="item in dataInfo.qualityControlManage">
        <el-form-item label="质控委员名：" class="flex-100">
            <span class="itemval" v-text="item.doctorName"></span>
        </el-form-item>
        <el-form-item label="质控委员评分：" class="flex-100">
            <span class="itemval" v-text="item.qualityLevel"></span>
        </el-form-item>
        <el-form-item label="质控委员评语：" class="flex-100">
            <span class="itemval" v-text="item.repayIllnessDescription"></span>
        </el-form-item>
      </template>
      <template v-if="dataInfo.isQuality==false">
        <el-form-item label="操作：" class="flex-100">
            <el-radio-group v-model="getType">
                <el-radio :label="1">评分</el-radio>
                <el-radio :label="2" @click="qualitySet()">转给质控委员</el-radio>
            </el-radio-group>
        </el-form-item>
        <el-form-item v-if="getType==1" label="评分：" class="flex-100">
            <el-radio-group v-model="grade" >
                <el-radio :label="1">1分</el-radio>
                <el-radio :label="2">2分</el-radio>
                <el-radio :label="3">3分</el-radio>
                <el-radio :label="4">4分</el-radio>
                <el-radio :label="5">5分</el-radio>
             </el-radio-group>
        </el-form-item>
        <el-form-item style="margin-top:10px;" v-if="getType==1" label="评价原因描述" class="flex-100" prop="reson">
        <div class="reasonbox"> 
          <el-input type="textarea" :rows="6" placeholder="请描述评价原因" v-model="reson" class="yh-width-fixed"></el-input>
        </div>
      </el-form-item>
      </template>
    </el-form>       
    <div class="form-footer">
        <el-button v-if="getType==1&&dataInfo.isQuality==false" type="primary"  @click="gradeget()"
           >确定</el-button>
        <router-link class="qualitybtn" v-if="getType==2&&state!='untreated'&&dataInfo.isQuality==false" target="_blank" :to="{path:'/qualityuser/list',query:{type:'quality',id:this.id}}">确定</router-link>
        <el-button v-if="dataInfo.refundCheckState==''" type="success"  @click.native="returnDialogVisible=true"
           >退单</el-button>
    </div>
    <!--图片查看视频播放-->
    <el-dialog :visible.sync="ImgVdoDialog" :title="mediaoftype=='image'?'图片查看':'视频播放'">
      <div v-if="mediaoftype=='image'"><img :src="ImgVdoUrl" width="100%"></div>
      <div v-else-if="mediaoftype=='video'"><video :src="ImgVdoUrl" controls width="100%" autoplay></video></div>
    </el-dialog> 
    <!-- 退单 -->
    <el-dialog
      class="shadebox"
      :title="'退单'"
      :visible.sync="returnDialogVisible"
      width="30%"
      height="400px"
      center>
      <el-form ref="form" label-width="100px">
        <el-form-item label="原因：" class="flex-100">
          <el-select placeholder="请选择退单原因" v-model="rebackcode">
               <el-option v-for="(item,key) in resonType" :key="key" :value="item.id" :label="item.labelName"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="退单描述：">
          <el-input placeholder="请填写退单原因描述" type="textarea" :rows="6" v-model="rebackreason" resize="none"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="returnDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="returnApply()">确 定</el-button>
      </span>
    </el-dialog>            
</section>
</template>
<script>
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns],
  data() {
    return {
      ImgVdoDialog:false,
      returnDialogVisible:false,//退单
      ImgVdoUrl:'',
      mediaoftype:'',
      resonType:[],
      getType:1,
      grade:'',
      reson:'',
      rebackreason:'',
      rebackcode:'',
      state:'',
      dataInfo:{
      }
    }
  },
  methods:{
    viewOpen(url,type){
      this.mediaoftype=type;
      this.ImgVdoDialog=true;
      this.ImgVdoUrl=url;
    },
    gradeget(){
      if(this.grade==""){
        this.errorMessage("评分不能为空");
        return false;
      }
      if(this.reson.trim()==""){
        this.errorMessage("评价原因不能为空");
        return false;
      }
      this.handleDataInfo(this.$api.chargeappraise,{id:this.dataInfo.evaluationId,qualityLevel:this.grade,qualityReason:this.reson},"/qualityorder/list");
    },
    qualitySet(){
      if(this.state=="untreated"){
        this.errorMessage("此订单已转给质控委员，订单在未处理状态不能转给其他质控委员");
      }
    },
    typeCheck(type){
      if(type=='Phone'){return '电话'}else if(type=='ImageText'){return '图文'}
    },
    returnApply(){//退单提交
      if(this.rebackcode==""){
        this.errorMessage("请选择退单原因");
        return false;
      }
      if(this.rebackreason.trim()==""){
        this.errorMessage("退单原因描述不能为空");
        return false;
      }
      this.$http.post(this.$api.referBack,{labelId:this.rebackcode,consultID:this.dataInfo.id,refundRemarks:this.rebackreason}).then(res=>{
        if(res.result.code==200){
          this.successMessage("操作成功");
          this.returnDialogVisible=false;
          this.routerLink('/qualityorder/list','list',0)
        }
      }).catch(error=>{
        this.errorMessage("操作失败");
        this.returnDialogVisible=false;
      });
    },
  },
  created(){
    this.$http.post(this.$api.referDetail,{id:this.$route.query.id}).then(res=>{
      if(res.result.code==200){
        this.dataInfo=res.result.item;
        if(this.dataInfo.qualityControlManage!=null){
          if(this.dataInfo.qualityControlManage.length>0){
            this.state=this.dataInfo.qualityControlManage[0].replyState;
          }
        }
        this.loading=false;
      }
    }).catch(error => { this.loading = false; });
    this.$http.post(this.$api.labelRelative,{labelTypeCode:'QualityControlReturnLabel'}).then(res=>{
      if(res.result.code==200){
        this.resonType=res.result.item[0].children;
        this.loading=false;
      }
    }).catch(error => { this.loading = false; });
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.userimages{float: left;}
.qualitybtn{background-color: #00aeff;border-color: #00aeff;padding: 8px 13px;min-width: 80px;font-weight: normal;color: #fff;font-size: 14px;border-radius: 4px;}
</style>