<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="类型模板" class="flex-100">
          <span class="itemval" v-text="dataInfo.templateCode"></span>
      </el-form-item>
      <el-form-item label="操作类型" class="flex-100" prop="operationType">
          <el-select placeholder="请选择操作类型" v-model="dataInfo.operationType">
              <el-option v-for="(item,key) in codeType" :key="key" :value="item.code" :label="item.name"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="接收人" class="flex-100" prop="recipient">
          <el-select placeholder="请选择接收人" v-model="dataInfo.recipient">
              <el-option v-for="(item,key) in nameType" :key="key" :value="item.code" :label="item.name"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="回访Url" class="flex-100" prop="backUrl">
          <el-input v-model="dataInfo.backUrl" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="firstMessage" class="flex-100" prop="firstMessage">
          <el-input v-model="dataInfo.firstMessage" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="keyword1" class="flex-100" prop="keyword1">
          <el-input v-model="dataInfo.keyword1" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="keyword2" class="flex-100" prop="keyword2">
          <el-input v-model="dataInfo.keyword2" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="keyword3" class="flex-100" prop="keyword3">
          <el-input v-model="dataInfo.keyword3" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="messageRemark" class="flex-100">
          <el-input v-model="dataInfo.messageRemark" class="yh-width-fixed"></el-input>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" @click="update()"
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
      codeType:[],
      nameType:[],
      rules: {//校验规则
        operationType: [required],
        recipient: [required],
        backUrl: [required],
        firstMessage: [required],
        keyword1: [required],
        keyword2: [required],
        keyword3: [required]
      },
      paramStatus:{
        type:'ConfigPar',
        systemCode:'OperationType'
      },
      nameStatus:{
        type:'ConfigPar',
        systemCode:'Recipient'
      }
    };
  },
  methods: {
    updatesendDataInfo(url, params, backUrl) {
        let form = this.$refs['form'];
        form.validate((valid) => {//验证方法
            if (valid) {
                this.loading = true;
                axios.post(url, params)
                    .then(res => {
                        this.loading = false;
                        if (res.result.code == "200") {
                            this.successMessage('修改成功');
                            if(backUrl==''){
                                return false;
                            }else{
                                setTimeout(() => {
                                    this.routerLink(backUrl,'',this.dataInfo.templateCode);
                                }, 1000);
                            }
                            
                        }
                    }).catch(error => { this.loading = false; })
            } else {
                return false
            }
        })
    },
    update() {
      this.updatesendDataInfo(this.$api.WXModelSendUpdate,this.dataInfo,"sendlist");
    }
  },
  created(){
    axios
        .all([
          axios.post(this.$api.baseParams,this.paramStatus), //操作类型
          axios.post(this.$api.baseParams,this.nameStatus), //接受人 
          axios.post(this.$api.WXModelSendData,{id:this.$route.query.id}), //默认数据 
        ])
        .then(
          axios.spread((type,name,data) => {
            this.codeType=type.result.item;
            this.nameType=name.result.item;
            this.dataInfo=data.result.item;
            this.loading = false;
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