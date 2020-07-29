<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button type="primary" 
             icon="el-icon-plus"
              @click="routerLink('/user/save','save',0)"
              >添加</el-button>
            <!-- 右操作 -->
             <div>
                <el-select v-model="params.queryStatus" @change="query">
                     <el-option label="状态" value=""></el-option>
                     <el-option label="已审核" value="0"></el-option>
                     <el-option label="禁用" value="1"></el-option>
                     <el-option label="未审核" value="2"></el-option>
                </el-select>
                 <yh-input  label="用户名" v-model="params.queryUsername"></yh-input>
                <yh-input  label="电子邮箱"  v-model="params.queryEmail"></yh-input>
                <el-button @click="query">查询</el-button>                
            </div>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%" @selection-change="checkIds">
               <el-table-column type="selection" width="38" align="center"></el-table-column>
               <el-table-column prop="Enabled" label="是否激活"  align="center">
                  <div slot-scope="scope">
                    <el-switch v-model="scope.row.act" @change="manageact($api.topicDelete,scope.row.id,scope.row.act)"></el-switch>
                  </div>
                </el-table-column>
                <el-table-column prop="id" label="id" width="50" align="center"></el-table-column>
                <el-table-column prop="name" label="专题名称" width="180" align="center"></el-table-column>
                <el-table-column prop="priority" label="排列顺序"  align="center"> 
                   <div slot-scope="scope">
                        <el-input v-model="scope.row.priority" type="number" class="w50"></el-input>
                   </div>
                </el-table-column>
                <el-table-column  label="操作"  align="center">
                  <div slot-scope="scope">
                      <yh-button type="edit" @click.native="routerLink('/topic/update','update',scope.row.id)"
                      
                      ></yh-button>
                       <yh-button type="delete" @click.native="deleteBatch($api.topicDelete,scope.row.id)"
                       ></yh-button>
                  </div>
                 </el-table-column>
           </el-table>
        </div>
        <!-- 表格底部 -->
        <div class="yh-list-footer">
                <div class="yh-left">                 
                  <el-button :disabled="disabled" @click="deleteBatch($api.topicDelete,ids)"                 
                  >批量删除</el-button>
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
      tableData:[{"titleImg":"/u/cms/www/201610/10100951y2xy.jpg","keywords":"","initials":"","name":" 2016饲料英才网络招聘会","description":"","recommend":true,"id":1,"shortName":"饲料英才网络招聘会","priority":1,"channelIds":[],"contentImg":"/u/cms/www/201610/11092540p27t.jpg","tplContent":""},{"titleImg":"/u/cms/www/201610/101010021q7v.jpg","keywords":"","initials":"","name":"2015全国两会","description":"","recommend":true,"id":3,"shortName":"2015全国两会","priority":11,"channelIds":[],"contentImg":"/u/cms/www/201610/11092148co74.jpg","tplContent":""},{"titleImg":"/u/cms/www/201610/10100842hqdk.jpg","keywords":"","initials":"","name":"互联网+与传统产业升级之道","description":"","recommend":true,"id":2,"shortName":"互联网+","priority":12,"channelIds":[],"contentImg":"/u/cms/www/201610/110911592mxa.jpg","tplContent":""}],
      params: {//只需要业务参数
        pageNo: "",
        pageSize: ""
      }
    };
  },
  methods:{
        priorityBatch(url) { //保存排列循序
        let params={
            ids:this.ids,
            priorities:[]
        }
        for(let i in this.checkedList){
            params.priorities.push(this.checkedList[i].priority);
        }
        
         params.priorities=params.priorities.join(',');
            this.$confirm('是否保存?', '提示', { type: "warning" })   
                .then(mes => {
                    this.$http.post(url, params).then(res => {
                        if (res.code == "200") {
                            this.successMessage('操作成功');
                            this.getTableData(this.params);
                        }
                    });
                })
                .catch(error => {});
        },
  },
  created(){
    this.loading=false;
     //this.initTableData(this.$api.topicList,this.params);
  }
};
</script>

<style scoped>

</style>
