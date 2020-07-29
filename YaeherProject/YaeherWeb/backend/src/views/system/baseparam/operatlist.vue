<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button type="primary" 
             icon="el-icon-plus"
              @click="routerLink('/role/add','add',0)"
              >添加</el-button>
            <!-- 右操作 -->
             <div>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'"></yh-input>
                <el-date-picker
                    v-model="dateArr"
                    value-format="yyyy-MM-dd"
                    :editable='false'
                    @change="rangeTime"
                    type="daterange"
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期">
                </el-date-picker>
                <el-button @click="query">查询</el-button>                
            </div>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column prop="roleName" label="角色名称" width="160" align="center"></el-table-column>
                <el-table-column prop="description" label="备注" align="center"> 
                </el-table-column>
                <el-table-column prop="Enabled" label="是否激活" width="120" align="center">
                  <div slot-scope="scope">
                    <label v-if="scope.row.enabled">是</label><label v-else="scope.row.enabled">否</label>
                  </div>
                </el-table-column>
                <el-table-column prop="isAdmin" label="是否管理员" width="120" align="center"> 
                  <div slot-scope="scope">
                    <label v-if="scope.row.isAdmin">是</label><label v-else="scope.row.isAdmin">否</label>
                  </div>
                </el-table-column>
                <el-table-column prop="createdOn" label="新增时间"  align="center"> 
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="user" @click.native="routerLink('/role/powerset','update',scope.row.id)"
                      ></yh-button>
                      <yh-button type="edit" @click.native="routerLink('/role/update','update',scope.row.id)"
                      ></yh-button>
                       <yh-button type="delete" @click.native="deleteBatch($api.roleDelete,scope.row.id)"
                       ></yh-button>
                  </div>
                 </el-table-column>
           </el-table>
        </div>
        <!-- 表格底部 -->
        <div class="yh-list-footer">
                <div class="yh-left">                 
                  <!-- <el-button :disabled="disabled" @click="deleteBatch($api.topicDelete,ids)"                 
                  >批量删除</el-button> -->
                </div>
                <!-- 分页组件 -->
              <yh-pagination :total="total" @change="getPages"></yh-pagination>
        </div> 
  </section>
</template>

<script>
import listMixins from '@/mixins/list'
export default {
  mixins:[listMixins],
  data() {
    return {
      dateArr:'',
      params: {//只需要业务参数
        keyWord:'',
        startTime: '',
        endTime: '',
        skipCount:1,
        maxResultCount:10
      }
    };
  },
  methods:{
        rangeTime(val){//格式化时间
          if(val!=null){
               this.params.startTime=val[0];
               this.params.endTime=val[1]; 
                if(val[0]===val[1]){
                    this.time=val[0];   
                }else{
                    this.time=val[0]+'-'+val[1];   
                }    
          }else{
            this.params.startTime='';
            this.params.endTime=''; 
            this.time=this.date.year+'-'+this.date.month+'-'+this.date.day  
          }       
        },
  },
  created(){
    this.initTableData(this.$api.roleList,this.params);
  }
};
</script>

<style scoped>

</style>
