<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="模板名称" class="flex-100" prop="names">
          <el-input placeholder="请输入模板名称" v-model="dataInfo.names" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="排序序号" class="flex-100" prop="orderSort">
          <el-input placeholder="请输入排序序号" v-model="dataInfo.orderSort" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="图标" class="flex-100" prop="icons">
          <el-input placeholder="请输入图标名称" v-model="dataInfo.icons" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="链接地址" class="flex-100" prop="linkUrls">
          <el-input placeholder="请输入链接地址" v-model="dataInfo.linkUrls" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="是否激活" class="flex-100">
        <template>
          <el-radio-group v-model="dataInfo.enabled">
            <el-radio :label="true">是</el-radio>
            <el-radio :label="false">否</el-radio>
          </el-radio-group>
        </template>
      </el-form-item>
      
    </el-form>
    <div class="form-footer">
        <el-button type="primary" v-if="isType('update')" @click="update()"
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
    let chinese=va.checkChinese();
    return {
      menuroot:[],
      rules: {//校验规则
        names: [required],
        orderSort: [required,number],
        //parentId: [required],
        icons: [required,chinese],
        linkUrls: [required,chinese],
        enabled: [required]
      }
    };
  },
  methods: {
    update() {
      if(this.dataInfo.parentId==0){
        this.dataInfo.isMenu=false;
      }else{
        this.dataInfo.isMenu=true;
      }
      this.updateDataInfo(this.$api.menuUpdate,this.dataInfo,"list");
    }
  },
  created(){
    this.getDataInfo(this.$api.menuData,{id:this.$route.query.id});
    axios.post(this.$api.menuRoot,this.params)
    .then(res=>{
        this.menuroot=res.result.item;
    })
    .catch(error=>{
        this.errorMessage('获取上级菜单失败')
    });
  }
};
</script>

<style scoped>
</style>