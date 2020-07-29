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
      <el-form-item label="医生姓名：" class="flex-50">
          <span class="itemval" v-text="dataInfo.doctorName"></span>
      </el-form-item>
      <el-form-item label="用户评分：" class="flex-50">
          <span class="itemval" v-text="dataInfo.evaluateLevel"></span>
      </el-form-item>
      <el-form-item label="评论标签：" class="flex-50">
          <span class="itemval" v-text="dataInfo.evaluateLabel"></span>
      </el-form-item>
      <el-form-item label="用户评论：" class="flex-50">
          <span class="itemval" v-text="dataInfo.evaluateContent"></span>
      </el-form-item>
      <el-form-item label="质控评分：" class="flex-50">
          <span class="itemval" v-text="dataInfo.qualityLevel"></span>
      </el-form-item>
    </el-form>       
    <div class="form-footer">
        <el-button type="success"  @click="$router.go(-1)"
           >返回</el-button>
        <el-button type="primary" @click.native="routerLink('/refer/reply','update',dataInfo.consultID)">咨询回复</el-button>
        <el-button type="primary" @click.native="routerLink('/refer/view','view',dataInfo.consultID)">查看咨询详情</el-button>
    </div>           
</section>
</template>
<script>
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns],
  data() {
    return {
      dataInfo:{
      }
    }
  },
  created(){
    this.getDataInfo(this.$api.appraiseData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.imagebox .userimages{margin:0px;display: inline-block;float: left;}
.imagebox .itemval{float: left;margin-left: 6px;}
</style>