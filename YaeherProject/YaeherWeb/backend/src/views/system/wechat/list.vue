<template>
  <section v-loading="loading" class="yh-body">
    <div class="yh-list-header flex-between">
        <el-button type="primary" 
        icon="el-icon-plus"
        @click="routerLink('/wechat/add','add',0)"
          >添加</el-button>
        <!-- 右操作 -->
        <div class="yh-list-hright">
            <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" :dataType="pagetype" @keyup.enter.native="search"></yh-input>
            <el-button @click="query('nopage')">查询</el-button>                
        </div>
    </div>
    <div class="yh-container">
        <tree-table :data="tableData" :eval-func="func" border>
          <el-table-column label="用户角色" align="center" prop="roleName">
          </el-table-column>
          <el-table-column label="用户编码" align="center" prop="roleCode">
          </el-table-column>
          <el-table-column label="自定义名称" align="center" prop="conditionalName">
          </el-table-column>
          <el-table-column label="自定义类型" align="center" prop="conditionalType">
          </el-table-column>
          <el-table-column label="自定义类型名称" align="center" prop="conditionalTypeName">
          </el-table-column>
          <el-table-column label="自定义路径" align="center" prop="conditionalUrl">
          </el-table-column>
          <el-table-column label="页面路径" align="center" prop="pagepath">
          </el-table-column>
          <el-table-column label="操作" width="200" align="center">
            <template slot-scope="scope">
              <yh-button type="send" v-if="scope.row.parentID==0" :title="'更新至微信'" @click.native="sendfor(scope.row.roleCode)">
              </yh-button>
              <yh-button type="plus" v-if="scope.row.parentID==0" :title="'添加标签'" @click.native="routerLink('/wechat/add',scope.row.roleCode+','+scope.row.roleName,scope.row.id)">
              </yh-button>
              <yh-button type="edit" @click.native="routerLink('/wechat/update','update',scope.row.id)">
              </yh-button>
              <yh-button type="delete" @click.native="deleteBatch($api.wechatDelete,scope.row.id,'nopage')">
              </yh-button>
            </template>
          </el-table-column>
        </tree-table>
    </div>
    <!-- 表格底部 -->
    <div class="yh-list-footer">
            <div class="yh-left">                 
              <!-- <el-button :disabled="disabled" @click="deleteBatch($api.topicDelete,ids)"                 
              >批量删除</el-button> -->
            </div>
            <!-- 分页组件 -->
          <!-- <yh-pagination :total="total" @change="getPages"></yh-pagination> -->
    </div> 
  </section>
</template>
<script>
import treeTable from '@/components/TreeTable'
import treeToArray from '@/untils/eval'
import listMixins from '@/mixins/list'
export default {
  name: 'menuManager',
  mixins:[listMixins],
  components: { treeTable },
  data() {
    return {
      menuroot:[],
      pagetype:'nopage',
      params:{
        keyWord:'',
        skipCount: 1,
        maxResultCount: 100
      },
      func: treeToArray
    }
  },
  methods:{
    sendfor(code){
      
      this.$confirm('是否确定更新至微信？', '警告', { type: "error" })
      .then(mes => {
          this.$http.post(this.$api.wechatSend,{roleCode:code}).then(res=>{
            if(res.result.code==200){
              this.successMessage("更新至微信成功！");
            }
          }).catch(error=>{
            this.errorMessage("操作失败");
          });
      })
      .catch(error => { });
    }
  },
  created(){
    this.initTableData(this.$api.wechatList,this.params,'nopage');
  }
}
</script>