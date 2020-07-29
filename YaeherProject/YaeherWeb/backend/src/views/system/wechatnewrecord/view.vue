<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back> 
    <el-form  ref="form" :model="dataInfo"
        class="yh-form" label-width="150px">
      <el-form-item label="appid：" class="flex-100">
          <span class="itemval" v-text="dataInfo.appid"></span>
      </el-form-item>
      <el-form-item label="backUrl：" class="flex-100">
          <span class="itemval" v-text="dataInfo.backUrl"></span>
      </el-form-item>
      <el-form-item label="咨询单号：" class="flex-100">
          <span class="itemval" v-text="dataInfo.consultNumber"></span>
      </el-form-item>
      <el-form-item label="咨询姓名：" class="flex-100">
          <span class="itemval" v-text="dataInfo.consultantName"></span>
      </el-form-item>
      <el-form-item label="医生姓名：" class="flex-100">
          <span class="itemval" v-text="dataInfo.doctorName"></span>
      </el-form-item>
      <el-form-item label="miniprogram：" class="flex-100">
          <span class="itemval" v-text="dataInfo.miniprogram"></span>
      </el-form-item>
      <el-form-item label="msgID：" class="flex-100">
          <span class="itemval" v-text="dataInfo.msgID"></span>
      </el-form-item>
      <el-form-item label="msgType：" class="flex-100">
          <span class="itemval" v-text="dataInfo.msgType"></span>
      </el-form-item>
      <el-form-item label="pagepath：" class="flex-100">
          <span class="itemval" v-text="dataInfo.pagepath"></span>
      </el-form-item>
      <el-form-item label="templateCode：" class="flex-100">
          <span class="itemval" v-text="dataInfo.templateCode"></span>
      </el-form-item>
      <el-form-item label="templateId：" class="flex-100">
          <span class="itemval" v-text="dataInfo.templateId"></span>
      </el-form-item>
      <el-form-item label="toUser：" class="flex-100">
          <span class="itemval" v-text="dataInfo.toUser"></span>
      </el-form-item>
    </el-form>       
    <div class="form-footer">
        <el-button type="success"  @click="$router.go(-1)"
           >返回</el-button>
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
  methods:{
  },
  created(){
    this.getDataInfo(this.$api.WXNewsSendDetail,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
.imagebox .userimages{margin:0px;display: inline-block;float: left;}
.imagebox .itemval{float: left;margin-left: 6px;}
.imgbox{width: 80px;}
.imgbox img{width: 100%;}
.vdobox{width: 80px;}
.vdobox video{width: 100%;}
</style>