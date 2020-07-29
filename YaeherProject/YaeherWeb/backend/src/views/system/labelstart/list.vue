<template>
  <section v-loading="loading" class="yh-body">
    <div class="yh-list-header flex-between">
        <el-button type="primary" 
        icon="el-icon-plus"
        @click="routerLink('/labelstart/add','add',0)"
          >添加</el-button>
        <!-- 右操作 -->
        <div class="yh-list-hright">
            <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" :dataType="pagetype" @keyup.enter.native="search"></yh-input>
            <el-button @click="query('nopage')">查询</el-button>                
        </div>
    </div>
    <div class="yh-container">
        <tree-table :data="tableData" :eval-func="func" border>
          <el-table-column label="标签类型名称" align="center" prop="labelTypeName">
          </el-table-column>
          <el-table-column label="标签类型" align="center" prop="labelTypeCode">
          </el-table-column>
          <el-table-column label="标签名称" align="center" prop="labelName">
          </el-table-column>
          <el-table-column label="标签编号" align="center" prop="labelCode">
          </el-table-column>
          <el-table-column label="排序序号" align="center" prop="orderSort">
          </el-table-column>
          <el-table-column label="操作" width="200" align="center">
            <template slot-scope="scope">
              <yh-button type="plus" v-if="scope.row.parentId==0" :title="'添加标签'" @click.native="routerLink('/labelstart/add',scope.row.labelTypeCode+','+scope.row.labelTypeName,scope.row.id)">
              </yh-button>
              <yh-button type="edit" @click.native="routerLink('/labelstart/update','update',scope.row.id)">
              </yh-button>
              <yh-button type="delete" @click.native="deleteBatch($api.labelstartDelete,scope.row.id,'nopage')">
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
  created(){
    this.initTableData(this.$api.labelstartList,this.params,'nopage');
  }
}
</script>