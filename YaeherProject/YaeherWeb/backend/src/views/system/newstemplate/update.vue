<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="类型名称" class="flex-100" prop="templateCode">
          <el-select @change="getName" placeholder="请选择类型名称" v-model="dataInfo.templateCode">
              <el-option v-for="(item,key) in codeType" :key="key" :value="item.code" :label="item.name"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="微信模板名称" class="flex-100">
          <span class="itemval" v-text="dataInfo.wecharTitle"></span>
      </el-form-item>
      <el-form-item label="微信模板编号" class="flex-100">
          <span class="itemval" v-text="dataInfo.templateId"></span>
      </el-form-item>
      <el-form-item label="模板内容" class="flex-100">
          <span class="itemval" v-text="dataInfo.content"></span>
      </el-form-item>
      <el-form-item label="模板示例" class="flex-100">
          <span class="itemval" v-text="dataInfo.example"></span>
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
        templateCode: [required]
      },
      codeType:[],
      paramStatus:{
        type:'ConfigPar',
        systemCode:'MessageTemplate'
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.WXNewsModelUpdate,this.dataInfo,"list");
    },
    getName(val){
      const obj=this.codeType;
      for(var i=0;i<obj.length;i++){
        if(obj[i].code==val){
           this.dataInfo.title=obj[i].name;
        }
      };
    },
  },
  created(){
    //this.getDataInfo(this.$api.systemSetData,{id:this.$route.query.id});
    axios
        .all([
          axios.post(this.$api.baseParams,this.paramStatus),
          axios.post(this.$api.WXNewsModelData,{id:this.$route.query.id}), //默认数据 
        ])
        .then(
          axios.spread((base,data) => {
            this.codeType=base.result.item;
            this.dataInfo=data.result.item;
            this.loading=false;
          })
        )
        .catch(err => {
          this.errorMessage('获取数据失败');
          this.loading = false;
        });
  }
};
</script>

<style scoped>
</style>