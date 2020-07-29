<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="文章标题" class="flex-100" prop="paperTiltle">
          <el-input v-model="dataInfo.paperTiltle" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="文章附件地址" class="flex-100" prop="paperAddress">
          <el-input v-model="dataInfo.paperAddress" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item v-if="this.dataInfo.paperFrom=='consultation'" label="案例编号" class="flex-100">
          <a :href="url+'/#/case/view?type=view&id='+dataInfo.consultNumber" target="_blank" class="egclass" title="点击查看">查看案例</a>
      </el-form-item>
      <el-form-item label="文章内容" class="flex-100 yeditor" prop="paperContent">
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
      url:'',
      defaultContent:'',
      rules: {//校验规则
        paperTiltle: [required],
        paperContent: [required],
        consultNumber:[required]
      },
      ue:[]
    };
  },
  methods: {
    update() {
      this.dataInfo.paperContent=this.ue[0].getContent();
      this.updateDataInfo(this.$api.doctorarticleUpdate,this.dataInfo,"list");
    },
    getUeditor(ue,index){
      this.ue[index]=ue;//获取ue实例
    }
  },
  created(){
    this.loading = true;
    this.url=window.location.href.split('/#')[0];
    axios.post(this.$api.doctorarticleData,{id:this.$route.query.id})
        .then(res => {  
            if (res.result.code == "200") {
                this.dataInfo=res.result.item;
                this.defaultContent=this.dataInfo.paperContent;
            }
            this.loading = false;
        }).catch(error => { this.loading = false; })
  }
};
</script>

<style scoped>
</style>