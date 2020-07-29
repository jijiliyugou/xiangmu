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
      <el-form-item label="医生姓名：" class="flex-100">
          <span class="itemval" v-text="dataInfo.doctorName"></span>
      </el-form-item>
      <el-form-item label="申请科室：" class="flex-100">
          <span class="itemval" v-text="dataInfo.clinicName"></span>
      </el-form-item>
      <el-form-item label="审核状态：" class="flex-100">
          <span class="itemval" v-text="dataInfo.checkRes"></span>
      </el-form-item>
      <el-form-item label="新增时间：" class="flex-100">
          <span class="itemval" v-text="formatTime(dataInfo.createdOn)"></span>
      </el-form-item>
      <el-form-item label="资格证：" class="flex-100">
          <div class="imgbox"><img :src="dataInfo.qualificationcertificate"><div class="ivshage" @click="viewOpen(dataInfo.qualificationcertificate,'image')"><i class="el-icon-zoom-in"></i></div></div>
      </el-form-item>
      <el-form-item label="执业证：" class="flex-100">
          <div class="imgbox"><img :src="dataInfo.certificateofpractice"><div class="ivshage" @click="viewOpen(dataInfo.certificateofpractice,'image')"><i class="el-icon-zoom-in"></i></div></div>
      </el-form-item>
    </el-form>       
    <div class="form-footer">
        <el-button :loading="loading" type="danger" v-if="isType('Check')" @click="shagepage()"
           >不通过</el-button>
        <el-button :loading="loading" type="success" v-if="isType('Check')" @click="pass()"
           >通过</el-button>
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
      this.paramType={id:this.id,checkRes:'fail',checkRemark:this.describe};
      if(this.describe.trim()==''){
        this.errorMessage("请填写不通过理由！");
        return false;
      }
      this.$http.post(this.$api.cliniccheckPut,this.paramType).then(res=>{
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
      this.paramType={id:this.id,checkRes:'success'};
      this.$http.post(this.$api.cliniccheckPut,this.paramType).then(res=>{
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
    this.getDataInfo(this.$api.doctorclinicData,{id:this.$route.query.id});
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