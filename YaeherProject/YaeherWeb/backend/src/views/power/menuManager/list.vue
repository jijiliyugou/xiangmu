<template>
  <section v-loading="loading" class="yh-body">
    <div class="yh-list-header flex-between">
        <el-button type="primary" 
        icon="el-icon-plus"
        @click="routerLink('/menuManager/add','add',0)"
          >添加</el-button>
        <!-- 右操作 -->
        <div class="yh-list-hright">
            <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" :dataType="pagetype" @keyup.enter.native="search"></yh-input>
            <!-- <label class="yh-label">上级模板：</label>
            <el-select placeholder="上级模板" v-model="params.upperLevel">
                 <el-option label="全部" :value="0"></el-option>
                 <el-option v-for="(item,key) in menuroot" :key="key" :value="item.id" :label="item.names"></el-option>
            </el-select> -->
            <label class="yh-label">激活状态：</label>
            <el-select placeholder="激活状态" v-model="params.enabled" @change="query('nopage')">
                 <el-option label="全部" :value="''"></el-option>
                 <el-option label="是" :value="true"></el-option>
                 <el-option label="否" :value="false"></el-option>
            </el-select>
            <el-button @click="query('nopage')">查询</el-button>                
        </div>
    </div>
    <div class="yh-container">
        <tree-table :data="tableData" :eval-func="func" border>
          <el-table-column label="模板名称" align="center" prop="names">
          </el-table-column>
          <el-table-column label="排序序号" align="center" prop="orderSort">
          </el-table-column>
          <el-table-column label="模块" align="center" prop="id">
          </el-table-column>
          <el-table-column label="上级模块" align="center" prop="parentId">
          </el-table-column>
          <el-table-column label="图标" align="center" prop="icons">
          </el-table-column>
          <el-table-column label="链接地址" align="center" prop="linkUrls">
          </el-table-column>
          <el-table-column label="是否菜单" align="center">
            <div slot-scope="scope">
               <label v-if="scope.row.isMenu">是</label><label v-else="scope.row.isMenu">否</label>
            </div>
          </el-table-column>
          <el-table-column label="是否激活" align="center">
            <div slot-scope="scope">
               <label v-if="scope.row.enabled">是</label><label v-else="scope.row.enabled">否</label>
            </div>
          </el-table-column>
          <el-table-column label="操作" width="200" align="center">
            <template slot-scope="scope">
              <yh-button type="edit" @click.native="routerLink('/menuManager/update','update',scope.row.id)">
              </yh-button>
              <yh-button type="delete" @click.native="deleteBatch($api.menuDelete,scope.row.id,'nopage')">
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
import axios from "axios";
export default {
  name: 'menuManager',
  mixins:[listMixins],
  components: { treeTable },
  data() {
    return {
      menuroot:[],
      pagetype:'nopage',
      params:{
        upperLevel:0,
        enabled:'',
        keyWord:'',
        skipCount: 1,
        maxResultCount: 100
      },
      func: treeToArray
    }
  },
  methods: {
    /*message(row) {
      this.$message.info(row.LinkUrl)
    }*/
  },
  created(){
    this.initTableData(this.$api.menuList,this.params,'nopage');
    /*axios.post(this.$api.menuRoot,this.params)
    .then(res=>{
        this.menuroot=res.result.item;
    })
    .catch(error=>{
        this.errorMessage('获取上级菜单失败')
    });*/
  }
}
</script>