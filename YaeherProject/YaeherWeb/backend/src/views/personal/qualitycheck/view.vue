<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back> 
    <el-form  ref="form" :model="dataInfo"
        class="yh-form" label-width="150px">
      <el-form-item label="头像：" class="flex-100">
          <div>
             <img :src="dataInfo.userImage" class="userimages">
          </div>
      </el-form-item>
      <el-form-item label="医生姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.doctorName"></span>
      </el-form-item>
      <el-form-item label="职称：" class="flex-50">
          <span class="itemval" v-text="dataInfo.title"></span>
      </el-form-item>
      <el-form-item label="所在医院：" class="flex-50">
          <span class="itemval" v-text="dataInfo.hospitalName"></span>
      </el-form-item>
      <el-form-item label="科室：" class="flex-50">
          <span class="itemval" v-text="dataInfo.department"></span>
      </el-form-item>
      <el-form-item label="毕业学校：" class="flex-50">
          <span class="itemval" v-text="dataInfo.graduateSchool"></span>
      </el-form-item>
      <el-form-item label="工作年限：" class="flex-50">
          <span class="itemval" v-text="dataInfo.workYear"></span>
      </el-form-item>
      <el-form-item label="手机号码：" class="flex-50">
          <span class="itemval" v-text="dataInfo.phoneNumber"></span>
      </el-form-item>
      <el-form-item label="关联微信号：" class="flex-50">
          <span class="itemval" v-text="dataInfo.wechatNum"></span>
      </el-form-item>
      <el-form-item label="订单数量：" class="flex-50">
          <span class="itemval" v-text="dataInfo.orderTotal"></span>
      </el-form-item>
      <el-form-item label="退单数量：" class="flex-50">
          <span class="itemval" v-text="dataInfo.refundTotal"></span>
      </el-form-item>
      <el-form-item label="评分：" class="flex-50">
          <span class="itemval" v-text="dataInfo.doctorLevel"></span>
      </el-form-item>
      <el-form-item label="审核状态：" class="flex-50">
          <span class="itemval" v-if="dataInfo.checkState=='fail'">不通过</span>
          <span class="itemval" v-else-if="dataInfo.checkState=='fail'">通过</span>
          <span class="itemval" v-else-if="dataInfo.checkState=='fail'">审核中</span>
      </el-form-item>
      <el-form-item label="新增时间：" class="flex-50">
          <span class="itemval" v-text="formatTime(dataInfo.createdOn)"></span>
      </el-form-item>
      <el-form-item label="申请类型：" class="flex-100">
          <span class="itemval" v-text="dataInfo.applyState=='qualitystop'?'取消质控委员':'申请质控委员'"></span>
      </el-form-item>
    </el-form>       
    <div class="form-footer">
        <el-button :loading="loading" type="success" v-if="isType('Check')" @click="pass()"
           >通过</el-button>
        <el-button :loading="loading" type="danger" v-if="isType('Check')" @click="shagepage()"
           >不通过</el-button>
        <el-button type="success" v-if="isType('view')"  @click="$router.go(-1)"
           >返回</el-button>
    </div>
    <el-dialog
      title="不通过"
      :visible.sync="centerDialogVisible"
      width="30%"
      height="400px"
      center>
      <el-input type="textarea" rows="7" resize="none" placeholder="请描述不通过理由" v-model="describe"></el-input>
      <span slot="footer" class="dialog-footer">
        <el-button @click="centerDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="nopass()">确 定</el-button>
      </span>
    </el-dialog>  
     <!--图片查看视频播放-->
    <el-dialog :visible.sync="ImgVdoDialog" :title="mediaoftype=='image'?'图片查看':'视频播放'">
      <div v-if="mediaoftype=='image'"><img :src="ImgVdoUrl" width="100%"></div>
      <div v-else-if="mediaoftype=='video'"><video :src="ImgVdoUrl" controls width="100%"></video></div>
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
      ImgVdoUrl:'',
      mediaoftype:'',
      describe:'',
      paramType:{},
      centerDialogVisible: false,
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
    shagepage(){
      this.centerDialogVisible=true;
    },
    nopass(type){
      this.paramType={qualityCommitteeRegisterID:this.id,qualityState:'fail',checkRemark:this.describe};
      if(this.describe.trim()==''){
        this.errorMessage("请填写不通过理由！");
        return false;
      }
      this.$http.post(this.$api.qualitycheckPut,this.paramType).then(res=>{
        if (res.result.code == "200") {
            this.centerDialogVisible=false;
            this.successMessage('操作成功');
            setTimeout(() => {
                this.routerLink('list');
            }, 1000);
        }
      }).catch(error=>{
        this.errorMessage('操作失败');
      });
    },
    pass(){
      this.paramType={qualityCommitteeRegisterID:this.id,qualityState:'success'};
      this.$http.post(this.$api.qualitycheckPut,this.paramType).then(res=>{
        if (res.result.code == "200") {
            this.successMessage('操作成功');
            setTimeout(() => {
                this.routerLink('list');
            }, 1000);
        }
      }).catch(error=>{
        this.errorMessage('操作失败');
      });
    },
  },
  created(){
    this.getDataInfo(this.$api.qualitycheckData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.imgbox{width: 200px;height: 140px;border:1px solid #e5e5e5;}
.imgbox img{display: block;width: 100%;height: 100%;}
.ivshage{line-height: 140px;}
.userimages{float: left;}
</style>