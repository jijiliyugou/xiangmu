<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form ref="form" 
        class="yh-form" label-width="162px">
      <el-form-item label="标签设置：" class="flex-100" prop="RoleName">
          <el-checkbox-group v-model="checkedRole" @change="handleCheckedRolesChange">
              <el-checkbox v-for="role in roles" :label="role.id" :key="role.id">{{role.lableName}}</el-checkbox>
          </el-checkbox-group>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button type="primary" @click="update()"
           >确定</el-button>
    </div>           
</section>
</template>
<script>
import axios from "axios";
import listMixins from '@/mixins/list';
export default {
  mixins:[listMixins], 
  data() {
    return {
      checkAll: false,
      checkedRole:[],
      roles:'',
      isIndeterminate: true
    };
  },
  methods: {
    update() {
      this.loading=true;
      this.$http.post(this.$api.doctorSetlabel,{doctorID:this.$route.query.id,LableIDJSON:this.checkedRole.join(",")}).then(res=>{
        if(res.result.code==200){
          this.loading=false;
          this.successMessage('操作成功');
          setTimeout(() => {
              this.routerLink('list');
          }, 1000);
        }
      }).catch(error=>{});
    },
    handleCheckedRolesChange(value) {
      this.checkedRole=value;
      //let checkedCount = value.length;
      //this.checkAll = checkedCount === this.roles.length;
      //this.isIndeterminate = checkedCount > 0 && checkedCount < this.roles.length;
    },
    setDefaultCheck() {
      //默认选中
      this.$http.post(this.$api.doctorGetlabel,{doctorID:this.$route.query.id}).then(res=>{
        if(res.result.code==200){
          this.checkedRole=res.result.item.lableIDJSON.replace(/,$/,"").split(",").map(function(data){return +data;});        }
      }).catch(error=>{});
    }
  },
  created(){
    this.$http.post(this.$api.doctorlabelListnopage,{doctorID:this.$route.query.id}).then(res=>{
      this.roles=res.result.item;
      this.loading=false;
      this.setDefaultCheck();
    }).catch(error=>{});
  }
};
</script>

<style scoped>
</style>