<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back> 
    <el-form  ref="form" :model="dataInfo"
        class="yh-form" label-width="150px">
      <el-form-item label="头像：" class="flex-50">
            <div slot-scope="scope" class="imagebox">
               <img :src="dataInfo.userImage" class="userimagedetail">
               <div class="usershage" @click="viewOpen(dataInfo.userImage,'image')"><i class="el-icon-zoom-in"></i></div>
            </div>
      </el-form-item>
      <el-form-item label="医生姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.doctorName"></span>
      </el-form-item>
      <!-- <el-form-item label="医生住址：" class="flex-50">
          <span class="itemval" v-text="dataInfo.address"></span>
      </el-form-item> -->
      <el-form-item label="职称：" class="flex-50">
          <span class="itemval" v-text="dataInfo.title"></span>
      </el-form-item>
      <el-form-item label="毕业学校：" class="flex-50">
          <span class="itemval" v-text="dataInfo.graduateSchool"></span>
      </el-form-item>
      <el-form-item label="工作医院：" class="flex-50">
          <span class="itemval" v-text="dataInfo.hospitalName"></span>
      </el-form-item>
      <el-form-item label="工作年限：" class="flex-50">
          <span class="itemval" v-text="dataInfo.workYear"></span>
      </el-form-item>
      <el-form-item label="所在科室：" class="flex-50">
          <span class="itemval" v-text="dataInfo.department"></span>
      </el-form-item>
      <el-form-item label="是否相信中医：" class="flex-50">
          <span class="itemval" v-text="dataInfo.isBelieveTCM==true?'是':'否'"></span>
      </el-form-item>
      <el-form-item label="是否具有服务意识：" class="flex-50">
          <span class="itemval" v-text="dataInfo.isServiceConscious==true?'是':'否'"></span>
      </el-form-item>
      <el-form-item label="手机号码：" class="flex-50">
          <span class="itemval" v-text="dataInfo.phoneNumber"></span>
      </el-form-item>
      <el-form-item label="关联微信号：" class="flex-50">
          <span class="itemval" v-text="dataInfo.wechatNum"></span>
      </el-form-item>
      <el-form-item label="推荐人：" class="flex-50">
          <span class="itemval" v-text="dataInfo.recommenderName"></span>
      </el-form-item>
      <el-form-item label="审核结果：" class="flex-50">
          <span class="itemval" v-text="dataInfo.checkRes"></span>
      </el-form-item>
      <el-form-item label="认证状态：" class="flex-50">
          <span class="itemval" v-text="dataInfo.authCheckRes"></span>
      </el-form-item>
      <el-form-item label="考试状态：" class="flex-50">
          <span class="itemval" v-if="dataInfo.baseTestRes=='fail' || dataInfo.simTestRes=='fail'">不通过</span>
          <span class="itemval" v-else-if="dataInfo.baseTestRes=='success' && dataInfo.simTestRes=='success'">通过</span>
          <span class="itemval" v-else>未考试</span>
      </el-form-item>
      <el-form-item label="新增时间：" class="flex-100">
          <span class="itemval" v-text="formatTime(dataInfo.createdOn)"></span>
      </el-form-item>
      <el-form-item v-if="roleType('Manager')==true?false:true" v-for="(item,key) in dataInfo.file" :key="key" :label="swapType(item.typeDetail)" class="flex-50" style="margin-top:12px;">
        <viewer :images="item.address">
          <div class="imgbox"><img :data-source="item.address" :src="item.address.replace('cos.ap-guangzhou','picgz')+imgRez"></div>
        </viewer>
      </el-form-item>
      <!-- <el-form-item label="简介：" class="flex-50" v-if="roleType('Manager')==true?false:true">
          <span class="itemval" v-text="dataInfo.resume"></span>
      </el-form-item> -->
    </el-form>       
    <div class="form-footer">
        <el-button :loading="loading" type="success" v-if="isType('Check')" @click="pass()"
           >通过</el-button>
        <el-button :loading="loading" type="danger" v-if="isType('Check')" @click="shagepage()"
           >不通过</el-button>
        <el-button :loading="loading" type="success" v-if="isType('Authen')" @click="pass()"
           >通过</el-button>
        <el-button :loading="loading" type="danger" v-if="isType('Authen')" @click="shagepage()"
           >不通过</el-button>
        <el-button :loading="loading" type="success" v-if="isType('Test')" @click="pass()"
           >通过</el-button>  
        <el-button :loading="loading" type="danger" v-if="isType('Test')" @click="nopass()"
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
    swapType(type){
      if(type=='idcardup'){return '身份证正面：'}else if(type=='idcarddown'){return '身份证反面：'}else if(type=='certificateofpractice'){return '执业证：'}else if(type=='qualificationcertificate'){return '资格证：'}
    },
    shagepage(){
      this.centerDialogVisible=true;
    },
    nopass(type){
      if(this.type=='Check'){
        this.paramType={Id:this.id,AuthCheck:this.type,CheckRes:'fail',CheckRemark:this.describe};
        if(this.describe.trim()==''){
          this.errorMessage("请填写不通过理由！");
          return false;
        }
      }else if(this.type=='Authen'){
        this.paramType={Id:this.id,AuthCheck:this.type,AuthCheckRes:'fail',AuthCheckRemark:this.describe};
        if(this.describe.trim()==''){
          this.errorMessage("请填写不通过理由！");
          return false;
        }
      }else if(this.type=='Test'){
        this.paramType={Id:this.id,AuthCheck:this.type,BaseTestRes:'fail',SimTestRes:'fail'};
      }
      
      this.$http.post(this.$api.doctorCheck,this.paramType).then(res=>{
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
      if(this.type=='Check'){
        this.paramType={Id:this.id,AuthCheck:this.type,CheckRes:'success'};
      }else if(this.type=='Authen'){
        this.paramType={Id:this.id,AuthCheck:this.type,AuthCheckRes:'success'};
      }else if(this.type=='Test'){
        this.paramType={Id:this.id,AuthCheck:this.type,BaseTestRes:'success',SimTestRes:'success'};
      }
      this.$http.post(this.$api.doctorCheck,this.paramType).then(res=>{
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
    this.getDataInfo(this.$api.doctorDetial,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.imagebox{width: 30px;height: 30px;position: relative;cursor: pointer;border-radius: 100%;overflow: hidden;}
.imagebox:hover .usershage{display: block;}
.usershage{position: absolute;width: 100%;height: 100%;z-index: 9;background: rgba(0,0,0,.5);font-size: 16px;color:#fff;left: 0px;top:0px;text-align: center;line-height: 30px;display: none;}
.ivshage{line-height: 140px;}
.imgbox{width: 200px;height: 140px;border:1px solid #e5e5e5;}
.imgbox img{display: block;width: 100%;height: 100%;}
</style>