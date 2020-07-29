<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form ref="form" 
        class="yh-form" label-width="162px">
      <el-form-item label="角色分配：" class="flex-100" prop="RoleName">
          <el-checkbox-group v-model="checkedRole" @change="handleCheckedRolesChange">
              <el-checkbox v-for="role in roles" :label="role.id" :key="role.id">{{role.roleName}}</el-checkbox>
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
      this.$http.post(this.$api.userUpdaterole,{userID:this.$route.query.id,roleId:this.checkedRole.join(",")}).then(res=>{
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
      //当前用户所拥有的角色
      this.$http.post(this.$api.userGetrole,{userID:this.$route.query.id}).then(res=>{
        if(res.result.code==200){
          this.checkedRole=res.result.item.roleID.replace(/,$/,"").split(",").map(function(data){return +data;});        }
      }).catch(error=>{});
    }
  },
  created(){
    this.$http.post(this.$api.roleList,this.rolesInfo).then(res=>{
      this.roles=res.result.item.items;
      this.loading=false;
      this.setDefaultCheck();
    }).catch(error=>{});
  }
};
</script>

<style scoped>
</style>