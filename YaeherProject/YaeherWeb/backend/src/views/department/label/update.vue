<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="标签名称" class="flex-100" prop="lableName">
          <el-input v-model="dataInfo.lableName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="标签说明" class="flex-100" prop="lableRemark">
          <el-input v-model="dataInfo.lableRemark" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="排序序号" class="flex-100" prop="orderSort">
          <el-input placeholder="请输入排序序号" v-model="dataInfo.orderSort" class="yh-width-fixed"></el-input>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" v-if="isType('update')" @click="update()"
           >确定</el-button>
    </div>           
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
        lableName: [required],
        lableRemark: [required],
        orderSort: [required,number]
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.labelUpdate,this.dataInfo,"list");
    }
  },
  created(){
    this.getDataInfo(this.$api.labelData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
</style>