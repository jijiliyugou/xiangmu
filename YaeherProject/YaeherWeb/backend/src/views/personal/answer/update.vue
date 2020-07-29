<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="问题描述" class="flex-100" prop="descriptionTiltle">
          <el-input v-model="dataInfo.descriptionTiltle" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="问题回答" class="flex-100 yeditor" prop="answer">
        <div class="editorbox">
          <yh-editor :defaultMsg="defaultContent" :index="0" @ready="getUeditor"></yh-editor>
        </div>
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
      defaultContent:'',
      rules: {//校验规则
        descriptionTiltle: [required],
        answer: [required]
      },
      ue:[]
    };
  },
  methods: {
    update() {
      this.dataInfo.answer=this.ue[0].getContent();
      this.updateDataInfo(this.$api.answerUpdate,this.dataInfo,"list");
    },
    getUeditor(ue,index){
      this.ue[index]=ue;//获取ue实例 
    }
  },
  created(){
    this.loading = true;
    axios.post(this.$api.answerData,{id:this.$route.query.id})
        .then(res => {  
            if (res.result.code == "200") {
                this.dataInfo=res.result.item;
                this.defaultContent=this.dataInfo.answer;
            }
            this.loading = false;
        }).catch(error => { this.loading = false; })
  }
};
</script>

<style scoped>
</style>