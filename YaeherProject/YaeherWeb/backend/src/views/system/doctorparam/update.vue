<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="代码编号" class="flex-100" prop="doctorParaSetCode">
          <el-input v-model="dataInfo.doctorParaSetCode" class="yh-width-fixed" :disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="名称" class="flex-100" prop="doctorParaSetName">
          <el-input v-model="dataInfo.doctorParaSetName" class="yh-width-fixed" :disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="itemValue" class="flex-100" prop="itemValue">
          <el-input v-model="dataInfo.itemValue" class="yh-width-fixed"></el-input>
      </el-form-item>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" v-if="isType('update')" @click="update()"
           >确定</el-button>
    </div>
  </el-form>           
</section>
</template>
<script>
import axios from "axios";
import va from "@/rules";
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns], 
  data() {
    let required = va.required();
    let number = va.number();
    return {
      rules: {//校验规则
        itemValue:[required]
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.doctorparamUpdate,this.dataInfo,"list");
    }
  },
  created(){
    this.getDataInfo(this.$api.doctorparamData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
</style>