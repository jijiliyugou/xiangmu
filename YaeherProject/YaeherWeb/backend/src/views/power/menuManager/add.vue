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
      <el-form-item label="上级模板" class="flex-100" prop="parentId">
        <el-cascader
          :options="options"
          :change-on-select="true"
          v-model="selectedOptions">
        </el-cascader>
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
        <el-button type="primary" v-if="isType('add')" @click="update()"
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
      options: [],
      selectedOptions: [],
      dataInfo:{
        enabled:true
      },
      rules: {//校验规则
        names: [required],
        orderSort: [required,number],
        parentId: [required],
        icons: [required,chinese],
        linkUrls: [required,chinese],
        enabled: [required]
      }
    };
  },
  methods: {
    update() {
      if(this.selectedOptions.length==1){
        this.dataInfo.parentId=this.selectedOptions[0];
      }else if(this.selectedOptions.length==2){
        this.dataInfo.parentId=this.selectedOptions[1];
      }
      if(this.dataInfo.parentId==0){
        this.dataInfo.isMenu=false;
      }else{
        this.dataInfo.isMenu=true;
      }
      this.saveDataInfo(this.$api.menuAdd,this.dataInfo,"list");
    }
  },
  created(){
    axios.post(this.$api.menuRoot,this.params)
    .then(res=>{
        const arr=res.result.item;
        const newArr=[{value:0,label:'根目录'}];
        for(let i=0;i<=arr.length-1;i++){
           const obj={};
           obj.label=arr[i].names;
           obj.value=arr[i].id;
           if(arr[i].children.length>0){
             obj.children=[];
             for(let j=0;j<=arr[i].children.length-1;j++){
               const childA={};
               childA.label=arr[i].children[j].names;
               childA.value=arr[i].children[j].id;
               obj.children.push(childA);
             }
           }
           newArr.push(obj);
        }
        this.options=newArr; 
        this.loading=false;
    })
    .catch(error=>{
        this.errorMessage('获取上级菜单失败');
        this.loading=false;
    });
  }
};
</script>

<style scoped>
</style>