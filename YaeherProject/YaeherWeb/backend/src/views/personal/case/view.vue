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
      <el-form-item label="咨询时间：" class="flex-50">
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
    </el-form>       
    <div class="form-footer">
        <el-button type="success"  @click="$router.go(-1)"
           >返回</el-button>
        <el-button type="primary" @click.native="routerLink('/doctorarticle/add','consultation',$route.query.id)">编写文章</el-button>
    </div> 
    <!--图片查看视频播放-->
    <el-dialog :visible.sync="ImgVdoDialog" :title="mediaoftype=='image'?'图片查看':'视频播放'">
      <div v-if="mediaoftype=='image'"><img :src="ImgVdoUrl" width="100%"></div>
      <div v-else-if="mediaoftype=='video'"><video :src="ImgVdoUrl" controls width="100%" autoplay></video></div>
    </el-dialog>           
</section>
</template>
<script>
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns],
  data() {
    return {
      imgRez:localStorage.getItem('imRez')==undefined?'':localStorage.getItem('imRez'),
      ImgVdoDialog:false,
      ImgVdoUrl:'',
      mediaoftype:'',
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
    typeCheck(type){
      if(type=='Phone'){return '电话'}else if(type=='ImageText'){return '图文'}
    }
  },
  created(){
    this.getDataInfo(this.$api.referDetail,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.imagebox .userimages{margin:0px;display: inline-block;float: left;}
.imagebox .itemval{float: left;margin-left: 6px;}
</style>