<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form ref="form" 
        class="yh-form" label-width="162px">
      <el-form-item label="权限分配：" class="flex-100">
          <el-radio v-for="(role,key) in roles" :key="key" v-model="code" :label="role.code">{{role.name}}</el-radio>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button type="primary" @click="update()"
           >确定</el-button>
    </div>           
</section>
</template>
<script>
import listMixins from '@/mixins/list';
export default {
  mixins:[listMixins], 
  data() {
    return {
      code:'',
      roles:[],
    };
  },
  methods: {
    update() {
      this.loading=true;
      this.$http.post(this.$api.mobileUpdate,{UserID:this.$route.query.id,RoleName:this.code}).then(res=>{
        if(res.result.code==200){
          this.loading=false;
          this.successMessage('操作成功');
          setTimeout(() => {
              this.routerLink('list');
          }, 1000);
        }
      }).catch(error=>{});
    },
  },
  created(){
    this.$http.post(this.$api.mobileData,{UserID:this.$route.query.id}).then(res=>{
      this.roles=res.result.item.wecharRoles;
      this.code=res.result.item.roleName;
      this.loading=false;
    }).catch(error=>{});
  }
};
</script>

<style scoped>
</style>