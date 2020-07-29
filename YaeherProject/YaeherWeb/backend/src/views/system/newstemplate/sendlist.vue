<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button type="primary" 
             icon="el-icon-plus"
              @click.native="routerLink('/newstemplate/sendadd','sendlist',params.templateCode)"
              >添加</el-button>
              <el-button type="success"  @click="routerLink('/newstemplate/list')"
           >返回</el-button>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column prop="templateCode" label="模板类型" align="center"></el-table-column>
                <el-table-column prop="operationType" label="操作类型" align="center">
                </el-table-column>
                <el-table-column prop="recipient" label="接收人" align="center"> 
                </el-table-column>
                <el-table-column prop="backUrl" label="回访Url" align="center"> 
                </el-table-column>
                <el-table-column prop="firstMessage" label="firstMessage" align="center"> 
                </el-table-column>
                <el-table-column prop="keyword1" label="keyword1" align="center"> 
                </el-table-column>
                <el-table-column prop="keyword2" label="keyword2" align="center"> 
                </el-table-column>
                <el-table-column prop="keyword3" label="keyword3" align="center"> 
                </el-table-column>
                <el-table-column prop="messageRemark" label="messageRemark" align="center"> 
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="edit" @click.native="routerLink('/newstemplate/sendupdate','update',scope.row.id)"
                      ></yh-button>
                       <yh-button type="delete" @click.native="deleteBatch($api.WXModelSendDelete,scope.row.id,'nopage')"
                       ></yh-button>
                  </div>
                 </el-table-column>
           </el-table>
        </div>
        <!-- 表格底部 -->
  </section>
</template>

<script>
import listMixins from '@/mixins/list'
export default {
  mixins:[listMixins],
  data() {
    return {
      params: {//只需要业务参数
        keyWord:'',
        startTime: '',
        endTime: '',
        skipCount:1,
        templateCode:'',
        maxResultCount:99
      }
    }
  },
  methods:{
  },
  created(){
    this.params.templateCode=this.$route.query.id;
    this.initTableData(this.$api.WXModelSendList,this.params,"nopage");
  }
};
</script>
