<template>
  <section v-loading="loading" class="yh-body">
    <div class="yh-role">
      <div class="treebox"><ul id="treeDemo" class="ztree"></ul></div>
      <div class="form-footer">
          <el-button type="primary" @click="update()"
             >确定</el-button>
      </div> 
    </div>
  </section>
</template>

<script>
import listMixins from '@/mixins/list'
import "@/plugs/jquery.ztree.all.min";
export default {
  mixins:[listMixins],
  name: "yh-role",
  props: {
    perms: {
      default: function() {
        return [];
      }
    }
  },
  data() {
    return {
      treeObj: {},
      rolesInfo:'',
      zNodes: [],
      setting: {
        //配置
        check: {
          enable: true
        },
        data: {
          key: {
            name: "names"
          }
        },
        callback: {
          onClick: this.onShow,
          onCheck: this.creatRoles //获取点击的name
        },
        view: {
          dblClickExpand: false //屏蔽掉双击事件
        }
      }
    };
  },
  methods: {
    update(){
      this.loading=true;
      this.$http.post(this.$api.setcheckId,{roleId:this.$route.query.id,moduleId:this.rolesInfo}).then(res=>{
        if(res.result.code==200){
          this.successMessage('操作成功');
          setTimeout(() => {
              this.routerLink('list');
          }, 1000);
        }
      }).catch(error=>{});
    },
    onShow(e, treeId, treeNode) {
      //单击显示
      this.treeObj.expandNode(treeNode);
    },
    setDefaultCheck() {
      //当前角色所拥有的权限
      this.$http.post(this.$api.getcheckId,{roleId:this.$route.query.id}).then(res=>{
        this.rolesInfo=res.result.item.moduleId;
        var checkId=this.rolesInfo.split(",");
        for(var i=0;i<checkId.length;i++){
          this.treeObj.checkNode(this.treeObj.getNodeByParam( "id",checkId[i]), true);
        }
      }).catch(error=>{});
      
    },
    creatRoles(event, treeId, treeNode) {
      //构造权限
      let role = [];
      let nodes = this.treeObj.getCheckedNodes(true);

      for (let i in nodes) {
        role.push(nodes[i].id);
      }
      this.rolesInfo = role.join(",");
       return this.rolesInfo;
    },
    getRoleInfo() {
      return this.rolesInfo;
    }/*,
    formatRoles(obj){  
     for(let i in obj){
        let roleName = obj[i].role
        if(this.perms.indexOf(roleName) > -1){
          obj[i].checked = true;
        }else{
           obj[i].checked = false;
        } 
        if(obj[i].children&&obj[i].children.length>0){
            this.formatRoles(obj[i].children)
        }
     }
    }*/
  },
  created() {
    this.$nextTick(()=>{
      this.$http.post(this.$api.menuList,this.rolesInfo).then(res=>{
        this.zNodes=res.result.item;
        this.setting.check.chkboxType = { Y: "ps", N: "ps" };   
        this.treeObj = $.fn.zTree.init($("#treeDemo"), this.setting, this.zNodes);
        this.setDefaultCheck();
        this.loading=false;
      }).catch(error=>{});
    })
  }
};
</script>

<style scoped>
.treebox{padding-left: 60px;padding-top: 28px;}
</style>
