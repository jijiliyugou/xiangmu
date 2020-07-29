<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="回复名称" class="flex-100" prop="title">
          <el-input v-model="dataInfo.title" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="回复内容" class="flex-100" prop="content">
        <div class="reasonbox"> 
          <el-input type="textarea" :rows="22" placeholder="请输入快捷回复内容" v-model="dataInfo.content" class="yh-width-fixed" @keyup.native="keyon" @change="key"></el-input>
          <label class="lettertype">{{letterNum}}/{{maxLength}}</label>
        </div>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" v-if="isType('add')" @click="update()"
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
    return {
      maxLength:5000,
      letterNum:0,
      dataInfo:{
        content:''
      },
      rules: {//校验规则
        title: [required],
        content: [required]
      }
    };
  },
  methods: {
    keyon(){
      if(this.dataInfo.content!=undefined){
        this.letterNum=this.dataInfo.content.length;
        if(this.letterNum>this.maxLength){
          this.dataInfo.content=this.dataInfo.content.substring(0,this.maxLength);
          this.letterNum=this.dataInfo.content.length;
        }
      }
    },
    key(){
      if(this.dataInfo.content!=undefined){
        this.letterNum=this.dataInfo.content.length;
        if(this.letterNum>this.maxLength){
          this.dataInfo.content=this.dataInfo.content.substring(0,this.maxLength);
          this.letterNum=this.dataInfo.content.length;
        }
      }
    },
    update() {
      this.saveDataInfo(this.$api.fastreplyAdd,this.dataInfo,"list");
    }
  },
  created(){
    this.loading=false;
  }
};
</script>

<style scoped>
.yh-width-fixed{width: 580px;}
</style>