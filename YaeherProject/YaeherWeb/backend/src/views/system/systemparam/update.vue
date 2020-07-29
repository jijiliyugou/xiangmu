<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="systemType" class="flex-100" prop="systemType">
          <el-input v-model="dataInfo.systemType" class="yh-width-fixed" :disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="systemCode" class="flex-100" prop="systemCode">
          <el-input v-model="dataInfo.systemCode" class="yh-width-fixed" :disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="名称" class="flex-100" prop="name">
          <el-input v-model="dataInfo.name" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="code" class="flex-100" prop="code">
          <el-input v-model="dataInfo.code" class="yh-width-fixed" :disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="itemValue" class="flex-100" prop="itemValue">
          <el-input v-model="dataInfo.itemValue" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="备注" class="flex-100" prop="remark">
          <el-input v-model="dataInfo.remark" class="yh-width-fixed"></el-input>
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
        name: [required],
        itemValue:[required],
        /*systemType: [required],
        systemCode: [required],
        code: [required],*/
        remark: [required]
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.systemUpdate,this.dataInfo,"list");
    }
  },
  created(){
    this.getDataInfo(this.$api.systemData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
</style>