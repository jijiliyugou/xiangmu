<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back> 
    <el-form  ref="form" :model="dataInfo"
        class="yh-form" label-width="150px">
      <el-form-item label="咨询单号：" class="flex-50">
          <span class="itemval" v-text="dataInfo.consultNumber"></span>
      </el-form-item>
      <el-form-item label="医生头像：" class="flex-50">
          <div slot-scope="scope" class="imagebox">
             <img :src="dataInfo.doctorImage" class="userimagedetail">
          </div>
      </el-form-item>
      <el-form-item label="医生姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.doctorName"></span>
      </el-form-item>
      <el-form-item label="咨询姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.consultantName"></span>
      </el-form-item>
      <el-form-item label="患者姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.patientName"></span>
      </el-form-item>
      <!-- <el-form-item label="患者性别：" class="flex-50">
          <span class="itemval" v-text="dataInfo.sex==1?'男':'女'"></span>
      </el-form-item> -->
      <el-form-item label="患者评分：" class="flex-50">
          <span class="itemval" v-text="dataInfo.evaluateLevel"></span>
      </el-form-item>
      <el-form-item label="患者评论：" class="flex-50">
          <span class="itemval" v-text="dataInfo.evaluateContent"></span>
      </el-form-item>
      <el-form-item label="评价标签：" class="flex-50">
          <el-tag size="mini" v-for="(item,index) in labelArr" :key="index">{{item.LabelName}}</el-tag>
      </el-form-item>
      <el-form-item label="质控评分：" class="flex-50">
          <span class="itemval" v-text="dataInfo.qualityLevel"></span>
      </el-form-item>
      <el-form-item label="质控评语：" class="flex-50">
          <span class="itemval" v-text="dataInfo.qualityReason"></span>
      </el-form-item>
      <el-form-item label="新增时间：" class="flex-100">
          <span class="itemval" v-text="formatTime(dataInfo.createdOn)"></span>
      </el-form-item>
      <div class="form-footer">
        <el-button type="primary" v-if="isType('view')"  @click="routerLink('/qualityorder/consultdetail','view',dataInfo.consultID)"
           >查看咨询订单</el-button>
      </div> 
    </el-form>       
           
</section>
</template>
<script>
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns],
  data() {
    return {
      labelArr:[],
      dataInfo:{
      }
    }
  },
  methods:{
    viewOpen(url,type){
      this.mediaoftype=type;
      this.ImgVdoDialog=true;
      this.ImgVdoUrl=url;
    }
  },
  created(){
    this.$http.post(this.$api.appraiseview,{id:this.$route.query.id}).then(res=>{
      if(res.result.code==200){
        this.dataInfo=res.result.item;
        this.labelArr=JSON.parse(this.dataInfo.evaluateReason);
        this.loading=false;
      }
    }).catch(error => { this.loading = false; });
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.ivshage{line-height: 140px;}
.imgbox{width: 200px;height: 140px;border:1px solid #e5e5e5;}
.imgbox img{display: block;width: 100%;height: 100%;}
.el-tag--mini{margin-right: 4px;}
</style>