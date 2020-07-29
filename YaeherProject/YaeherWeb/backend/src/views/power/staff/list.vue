<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button type="primary" 
             icon="el-icon-plus"
              @click="routerLink('/staff/add','add',0)"
              >添加</el-button>
            <!-- 右操作 -->
            <div>
                <label class="yh-label">平台角色：</label>
                <el-select placeholder="平台角色" v-model="params.wecharRole" @change="query()">
                     <el-option label="全部" :value="''"></el-option>
                     <el-option label="患者" :value="'patient'"></el-option>
                     <el-option label="医生" :value="'doctor'"></el-option>
                     <el-option label="管理" :value="'admin'"></el-option>
                     <el-option label="质控" :value="'quality'"></el-option>
                     <el-option label="客服" :value="'customerservice'"></el-option>
                </el-select>
                <label class="yh-label">激活状态：</label>
                <el-select placeholder="激活状态" v-model="params.enabled" @change="query()">
                     <el-option label="全部" :value="''"></el-option>
                     <el-option label="是" :value="true"></el-option>
                     <el-option label="否" :value="false"></el-option>
                </el-select>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" @keyup.enter.native="search"></yh-input>
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
                <el-table-column label="头像" width="80" align="center">
                  <div slot-scope="scope">
                     <img :src="scope.row.userImage==null?'./../static/images/userlogo.png':scope.row.userImage.replace('cos.ap-guangzhou','picgz')+imgRez" class="userimages">
                  </div>
                </el-table-column>
                <el-table-column prop="fullName" label="姓名" align="center"> 
                </el-table-column>
                <el-table-column prop="loginName" label="系统登录名" align="center"> 
                </el-table-column>
                <el-table-column prop="nickName" label="微信昵称" align="center"> 
                </el-table-column>
                <el-table-column prop="phoneNumber" label="电话" align="center"> 
                </el-table-column>
                <el-table-column prop="email" label="邮箱" align="center"> 
                </el-table-column>
                <el-table-column label="性别" align="center" width="50"> 
                  <div slot-scope="scope">
                     <label v-if="scope.row.sex==1">男</label><label v-else="scope.row.sex==2">女</label>
                  </div>
                </el-table-column>
                <el-table-column prop="birthday" label="生日" align="center" width="100">
                  <div slot-scope="scope">
                    {{formatDate(scope.row.birthday)}}
                  </div> 
                </el-table-column>
                <el-table-column prop="userorigin" label="用户来源" align="center"> 
                </el-table-column>
                <el-table-column prop="wecharNo" label="关联微信" align="center"> 
                </el-table-column>
                <el-table-column prop="roleName" label="平台角色" align="center"> 
                  <div slot-scope="scope">
                    {{roleCheck(scope.row.roleName)}}
                  </div>
                </el-table-column>
                <el-table-column label="新增时间" align="center"> 
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}}
                  </div>
                </el-table-column>
                <el-table-column label="激活状态" align="center" width="80">
                  <div slot-scope="scope">
                     <label v-if="scope.row.enabled">是</label><label v-else="scope.row.enabled">否</label>
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center" width="120">
                  <div slot-scope="scope">
                      <yh-button type="user" @click.native="routerLink('/staff/roleset','update',scope.row.id)"
                      ></yh-button>
                      <yh-button type="edit" @click.native="routerLink('/staff/update','update',scope.row.id)"
                      ></yh-button>
                       <yh-button type="delete" @click.native="deleteBatch($api.userDelete,scope.row.id)"
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
  name:'keepin',
  mixins:[listMixins],
  data() {
    return {
      imgRez:localStorage.getItem('imRez')==undefined?'':localStorage.getItem('imRez'),
      dateArr:'',
      tableData:[],
      params: {//只需要业务参数
        wecharRole:'',
        keyWord:'',
        enabled:'',
        startTime: '',
        endTime: '',
        skipCount: 1,
        maxResultCount: 10
      }
    };
  },
  methods:{
    roleCheck(type){
      if(type=='doctor'){
        return '医生';
      }else if(type=='patient'){
        return '患者';
      }else if(type=='customerservice'){
        return '客服';
      }else if(type=='quality'){
        return '质控';
      }else if(type=='admin'){
        return '管理';
      }
    }
  },
  created(){
    
  },
  activated () {
    this.getRangeTime();
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.initTableData(this.$api.userList,this.params);
  },
  beforeRouteLeave (to, from, next) {
    let topath=to.path.split('/').pop();
    if(topath=='add'||topath=='list'){
      this.params.skipCount=1;
    }
    next();
  }
};
</script>

<style scoped>
</style>
