<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back> 
    <el-form  ref="form" :model="dataInfo"
        class="yh-form">
      <h1 class="papertitle" v-text="dataInfo.rulesTitle"></h1>
      <div class="paperbody" v-html="dataInfo.rulesContent">
        <img title="sdf.jpg" alt="sdf.jpg" src="http://192.168.2.3:8001/upload/userid/image/15441551929172148.jpg">
      </div>
    </el-form>       
    <!-- <div class="form-footer">
                  <el-button type="success"  @click="$router.go(-1)"
                     >返回</el-button>
              </div>   -->          
</section>
</template>
<script>
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns],
  data() {
    return {
    }
  },
  created(){
    this.getDataInfo(this.$api.regimeData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
.papertitle{width: 100%;text-align: center;}
.paperbody{width: 100%;overflow: hidden;margin-top: 24px;}
>>>img{display: block;width: 100%;}
>>>a{color:#4F81BD;font-size: 16px;text-decoration: underline;}
>>>a:hover{color:#f94f50;}
</style>