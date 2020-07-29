import Vue from 'vue'
import Router from 'vue-router'
import login from '@/components/login'
import body from '@/components/body'
import work from '@/components/work.vue'
import error from '@/components/404.vue'
import childView from '@/components/appMain.vue'
/*const resourceTree = resolve => { require(['@/views/interface/resource/tree.vue'], resolve) };
const resourceList = resolve => { require(['@/views/interface/resource/list.vue'], resolve) };
const resourceEdit = resolve => { require(['@/views/interface/resource/edit.vue'], resolve) };
const resourceReName = resolve => { require(['@/views/interface/resource/rename.vue'], resolve) };
const templateTree = resolve => { require(['@/views/interface/template/tree.vue'], resolve) };
const templateList = resolve => { require(['@/views/interface/template/list.vue'], resolve) };
const templateEdit = resolve => { require(['@/views/interface/template/edit.vue'], resolve) };
const templateReName = resolve => { require(['@/views/interface/template/rename.vue'], resolve) };
const templateSetting = resolve => { require(['@/views/interface/template/setting.vue'], resolve) }; */
/*function $importViews(componentName) { //异步加载组件
  return resolve => {
    require(['@/views/' + componentName + '.vue'], resolve)
  }
}*/


Vue.use(Router)
/**
 * routes==默认路由节点
 */
export const routes = [{
  path: '/login',
  name: '登录',
  component: login,
  hidden: true
},
{
  path: '/',
  name: '工作台',
  meta: {
    role: 'work'
  },
  component: body,
  iconCls: 'icon-iconfontdesktop',
  leaf: true, //只有一个节点
  redirect: '/work',
  children: [{
    path: '/work',
    component: work,
  }
  ]
}
];
export const ansycRoutes = [{
        path: '/moban',
        name: '模板管理', //模板管理
        component: body,
        iconCls: 'icon-iconfontdesktop',
        leaf: true, //只有一个节点
        redirect: '/moban/list',
        children: [{
            path: '/moban/list',
            name: '模板列表',
            component: resolve => {
                require(['@/views/moban/list.vue'], resolve)
            }
        }, {
            path: '/moban/add',
            name: '模板添加',
            component: resolve => {
                require(['@/views/moban/add.vue'], resolve)
            }
        }]
    },{
    path: '/system', //系统参数
    name: '系统参数',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/systemparam',
        name: '系统参数设置', //基础参数设置
        component: childView,
        isParent: true,
        redirect: '/systemparam/list',
        children: [{
            path: '/systemparam/list',
            name: '系统参数列表',
            component: resolve => {
                require(['@/views/system/systemparam/list.vue'], resolve)
            }
        }, {
            path: '/systemparam/add',
            name: '系统参数添加',
            component: resolve => {
                require(['@/views/system/systemparam/add.vue'], resolve)
            }
        }, {
            path: '/systemparam/update',
            name: '系统参数修改',
            component: resolve => {
                require(['@/views/system/systemparam/update.vue'], resolve)
            }
        }]
    },{
        path: '/doctorparam',
        name: '医生参数管理', //医生参数管理
        component: childView,
        isParent: true,
        redirect: '/doctorparam/list',
        children: [{
            path: '/doctorparam/list',
            name: '医生参数列表',
            component: resolve => {
                require(['@/views/system/doctorparam/list.vue'], resolve)
            }
        }, {
            path: '/doctorparam/add',
            name: '医生参数添加',
            component: resolve => {
                require(['@/views/system/doctorparam/add.vue'], resolve)
            }
        }, {
            path: '/doctorparam/update',
            name: '医生参数修改',
            component: resolve => {
                require(['@/views/system/doctorparam/update.vue'], resolve)
            }
        }]
    },{
        path: '/doctorstatus',
        name: '医生上下线设置', //医生上下线设置
        component: childView,
        isParent: true,
        redirect: '/doctorstatus/list',
        children: [{
            path: '/doctorstatus/list',
            name: '医生上下线列表',
            component: resolve => {
                require(['@/views/system/doctorstatus/list.vue'], resolve)
            }
        },{
            path: '/doctorstatus/update',
            name: '医生上下线编辑',
            component: resolve => {
                require(['@/views/system/doctorstatus/update.vue'], resolve)
            }
        }]
    },{
        path: '/port',
        name: '对接接口设置', //对接接口设置
        component: childView,
        isParent: true,
        redirect: '/port/list',
        children: [{
            path: '/port/list',
            name: '对接接口列表',
            component: resolve => {
                require(['@/views/system/port/list.vue'], resolve)
            }
        },{
            path: '/port/add',
            name: '对接接口添加',
            component: resolve => {
                require(['@/views/system/port/add.vue'], resolve)
            }
        },{
            path: '/port/update',
            name: '对接接口编辑',
            component: resolve => {
                require(['@/views/system/port/update.vue'], resolve)
            }
        }]
    },{
        path: '/notesend',
        name: '短信发送记录', //短信发送记录
        component: childView,
        isParent: true,
        redirect: '/notesend/list',
        children: [{
            path: '/notesend/list',
            name: '短信发送列表',
            component: resolve => {
                require(['@/views/system/notesend/list.vue'], resolve)
            }
        }]
    },{
        path: '/labelstart',
        name: '标签与星级', //标签与星级
        component: childView,
        isParent: true,
        redirect: '/labelstart/list',
        children: [{
            path: '/labelstart/list',
            name: '标签与星级列表',
            component: resolve => {
                require(['@/views/system/labelstart/list.vue'], resolve)
            }
        },{
            path: '/labelstart/add',
            name: '标签与星级添加',
            component: resolve => {
                require(['@/views/system/labelstart/add.vue'], resolve)
            }
        },{
            path: '/labelstart/update',
            name: '标签与星级编辑',
            component: resolve => {
                require(['@/views/system/labelstart/update.vue'], resolve)
            }
        }]
    },{
        path: '/swiper',
        name: '轮播图设置', //轮播图设置
        component: childView,
        isParent: true,
        redirect: '/swiper/list',
        children: [{
            path: '/swiper/list',
            name: '轮播图列表',
            component: resolve => {
                require(['@/views/system/swiper/list.vue'], resolve)
            }
        },{
            path: '/swiper/add',
            name: '轮播图添加',
            component: resolve => {
                require(['@/views/system/swiper/add.vue'], resolve)
            }
        },{
            path: '/swiper/update',
            name: '轮播图编辑',
            component: resolve => {
                require(['@/views/system/swiper/update.vue'], resolve)
            }
        }]
    },{
        path: '/wechat',
        name: '微信菜单', //微信菜单
        component: childView,
        isParent: true,
        redirect: '/wechat/list',
        children: [{
            path: '/wechat/list',
            name: '微信菜单列表',
            component: resolve => {
                require(['@/views/system/wechat/list.vue'], resolve)
            }
        },{
            path: '/wechat/add',
            name: '微信菜单添加',
            component: resolve => {
                require(['@/views/system/wechat/add.vue'], resolve)
            }
        },{
            path: '/wechat/update',
            name: '微信菜单编辑',
            component: resolve => {
                require(['@/views/system/wechat/update.vue'], resolve)
            }
        }]
    },{
        path: '/newstemplate',
        name: '消息模板', //消息模板
        component: childView,
        isParent: true,
        redirect: '/newstemplate/list',
        children: [{
            path: '/newstemplate/list',
            name: '消息模板列表',
            component: resolve => {
                require(['@/views/system/newstemplate/list.vue'], resolve)
            }
        },{
            path: '/newstemplate/update',
            name: '消息模板编辑',
            component: resolve => {
                require(['@/views/system/newstemplate/update.vue'], resolve)
            }
        },{
            path: '/newstemplate/sendlist',
            name: '发送消息模板列表',
            component: resolve => {
                require(['@/views/system/newstemplate/sendlist.vue'], resolve)
            }
        },{
            path: '/newstemplate/sendadd',
            name: '发送消息模板添加',
            component: resolve => {
                require(['@/views/system/newstemplate/sendadd.vue'], resolve)
            }
        },{
            path: '/newstemplate/sendupdate',
            name: '发送消息模板修改',
            component: resolve => {
                require(['@/views/system/newstemplate/sendupdate.vue'], resolve)
            }
        }]
    },{
        path: '/wechatnewrecord',
        name: '公众号消息发送记录', //公众号消息发送记录
        component: childView,
        isParent: true,
        redirect: '/wechatnewrecord/list',
        children: [{
            path: '/wechatnewrecord/list',
            name: '消息发送记录列表',
            component: resolve => {
                require(['@/views/system/wechatnewrecord/list.vue'], resolve)
            }
        },{
            path: '/wechatnewrecord/view',
            name: '消息发送记录详情',
            component: resolve => {
                require(['@/views/system/wechatnewrecord/view.vue'], resolve)
            }
        }]
    },{
        path: '/systemset',
        name: '系统配置', //系统配置
        component: childView,
        isParent: true,
        redirect: '/systemset/list',
        children: [{
            path: '/systemset/list',
            name: '系统配置列表',
            component: resolve => {
                require(['@/views/system/systemset/list.vue'], resolve)
            }
        },{
            path: '/systemset/update',
            name: '系统配置修改',
            component: resolve => {
                require(['@/views/system/systemset/update.vue'], resolve)
            }
        }]
    }]
    },{
    path: '/power', //权限管理
    name: '权限管理',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/role',
        name: '角色管理', //用户管理
        component: childView,
        isParent: true,
        redirect: '/role/list',
        children: [{
            path: '/role/list',
            name: '角色列表',
            component: resolve => {
                require(['@/views/power/role/list.vue'], resolve)
            }
        }, {
            path: '/role/add',
            name: '角色添加',
            component: resolve => {
                require(['@/views/power/role/add.vue'], resolve)
            }
        }, {
            path: '/role/update',
            name: '角色修改',
            component: resolve => {
                require(['@/views/power/role/update.vue'], resolve)
            }
        }, {
            path: '/role/powerset',
            name: '权限设置',
            component: resolve => {
                require(['@/views/power/role/powerset.vue'], resolve)
            }
        }]
    },{
        path: '/menuManager',
        name: '菜单管理', //菜单管理
        component: childView,
        isParent: true,
        redirect: '/menuManager/list',
        children: [{
            path: '/menuManager/list',
            name: '菜单列表',
            component: resolve => {
                require(['@/views/power/menuManager/list.vue'], resolve)
            }
        }, {
            path: '/menuManager/add',
            name: '菜单添加',
            component: resolve => {
                require(['@/views/power/menuManager/add.vue'], resolve)
            }
        }, {
            path: '/menuManager/update',
            name: '菜单修改',
            component: resolve => {
                require(['@/views/power/menuManager/update.vue'], resolve)
            }
        }]
    },{
        path: '/staff',
        name: '人员信息', //人员信息
        component: childView,
        isParent: true,
        redirect: '/staff/list',
        children: [{
            path: '/staff/list',
            name: '人员列表',
            component: resolve => {
                require(['@/views/power/staff/list.vue'], resolve)
            }
        }, {
            path: '/staff/add',
            name: '人员添加',
            component: resolve => {
                require(['@/views/power/staff/add.vue'], resolve)
            }
        }, {
            path: '/staff/update',
            name: '人员修改',
            component: resolve => {
                require(['@/views/power/staff/update.vue'], resolve)
            }
        }, {
            path: '/staff/roleset',
            name: '分配角色',
            component: resolve => {
                require(['@/views/power/staff/roleset.vue'], resolve)
            }
        }]
    }]
  },{
    path: '/consult', //咨询管理
    name: '咨询管理',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/refer',
        name: '咨询列表', //咨询列表
        component: childView,
        isParent: true,
        redirect: '/refer/list',
        children: [{
            path: '/refer/list',
            name: '咨询列表',
            component: resolve => {
                require(['@/views/consult/refer/list.vue'], resolve)
            }
          },{
            path: '/refer/reply',
            name: '咨询回复',
            component: resolve => {
                require(['@/views/consult/refer/reply.vue'], resolve)
            }
          },{
            path: '/refer/view',
            name: '咨询详情',
            component: resolve => {
                require(['@/views/consult/refer/view.vue'], resolve)
            }
          },{
            path: '/refer/reback',
            name: '退单',
            component: resolve => {
                require(['@/views/consult/refer/reback.vue'], resolve)
            }
          }]
        },{
        path: '/order',
        name: '订单管理', //订单查看
        component: childView,
        isParent: true,
        redirect: '/order/list',
        children: [{
            path: '/order/list',
            name: '订单列表',
            component: resolve => {
                require(['@/views/consult/order/list.vue'], resolve)
            }
        },{
            path: '/order/view',
            name: '订单详情',
            component: resolve => {
                require(['@/views/consult/order/view.vue'], resolve)
            }
          }]
    },{
        path: '/payrecord',
        name: '支付记录', //支付记录
        component: childView,
        isParent: true,
        redirect: '/payrecord/list',
        children: [{
            path: '/payrecord/list',
            name: '支付列表',
            component: resolve => {
                require(['@/views/consult/payrecord/list.vue'], resolve)
            }
        },{
            path: '/payrecord/view',
            name: '支付详情',
            component: resolve => {
                require(['@/views/consult/payrecord/view.vue'], resolve)
            }
          }]
    },{
        path: '/retreat',
        name: '退单管理', //退单查询
        component: childView,
        isParent: true,
        redirect: '/retreat/list',
        children: [{
            path: '/retreat/list',
            name: '退单列表',
            component: resolve => {
                require(['@/views/consult/retreat/list.vue'], resolve)
            }
        },{
            path: '/retreat/view',
            name: '退单详情',
            component: resolve => {
                require(['@/views/consult/retreat/view.vue'], resolve)
            }
          }]
    },{
        path: '/fastreply',
        name: '快捷回复', //快捷回复
        component: childView,
        isParent: true,
        redirect: '/fastreply/list',
        children: [{
            path: '/fastreply/list',
            name: '快捷回复列表',
            component: resolve => {
                require(['@/views/consult/fastreply/list.vue'], resolve)
            }
        },{
            path: '/fastreply/add',
            name: '快捷回复添加',
            component: resolve => {
                require(['@/views/consult/fastreply/add.vue'], resolve)
            }
          },{
            path: '/fastreply/update',
            name: '快捷回复编辑',
            component: resolve => {
                require(['@/views/consult/fastreply/update.vue'], resolve)
            }
          }]
    }]
  },{
    path: '/department', //科室与医生
    name: '科室与医生',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/doctor',
        name: '医生管理', //医生管理
        component: childView,
        isParent: true,
        redirect: '/doctor/list',
        children: [{
            path: '/doctor/list',
            name: '医生列表',
            component: resolve => {
                require(['@/views/department/doctor/list.vue'], resolve)
            }
        },{
            path: '/doctor/view',
            name: '医生查看',
            component: resolve => {
                require(['@/views/department/doctor/view.vue'], resolve)
            }
        },{
            path: '/doctor/partset',
            name: '医生科室设置',
            component: resolve => {
                require(['@/views/department/doctor/partset.vue'], resolve)
            }
        },{
            path: '/doctor/labelset',
            name: '医生标签设置',
            component: resolve => {
                require(['@/views/department/doctor/labelset.vue'], resolve)
            }
        }]
    },{
        path: '/label',
        name: '标签管理', //标签管理
        component: childView,
        isParent: true,
        redirect: '/label/list',
        children: [{
            path: '/label/list',
            name: '标签列表',
            component: resolve => {
                require(['@/views/department/label/list.vue'], resolve)
            }
          },{
            path: '/label/add',
            name: '标签添加',
            component: resolve => {
                require(['@/views/department/label/add.vue'], resolve)
            }
        },{
            path: '/label/update',
            name: '标签修改',
            component: resolve => {
                require(['@/views/department/label/update.vue'], resolve)
            }
        }]
        },{
        path: '/labelgroup',
        name: '标签关联组', //标签关联组
        component: childView,
        isParent: true,
        redirect: '/labelgroup/list',
        children: [{
            path: '/labelgroup/list',
            name: '标签关联组列表',
            component: resolve => {
                require(['@/views/department/labelgroup/list.vue'], resolve)
            }
          },{
            path: '/labelgroup/add',
            name: '标签关联组添加',
            component: resolve => {
                require(['@/views/department/labelgroup/add.vue'], resolve)
            }
        },{
            path: '/labelgroup/update',
            name: '标签关联组修改',
            component: resolve => {
                require(['@/views/department/labelgroup/update.vue'], resolve)
            }
        }]
        },{
        path: '/part',
        name: '科室管理', //科室管理
        component: childView,
        isParent: true,
        redirect: '/part/list',
        children: [{
            path: '/part/list',
            name: '科室列表',
            component: resolve => {
                require(['@/views/department/part/list.vue'], resolve)
            }
        },{
            path: '/part/add',
            name: '科室添加',
            component: resolve => {
                require(['@/views/department/part/add.vue'], resolve)
            }
        },{
            path: '/part/update',
            name: '科室修改',
            component: resolve => {
                require(['@/views/department/part/update.vue'], resolve)
            }
        },{
            path: '/part/doctorset',
            name: '科室医生设置',
            component: resolve => {
                require(['@/views/department/part/doctorset.vue'], resolve)
            }
        },{
            path: '/part/labelset',
            name: '科室标签设置',
            component: resolve => {
                require(['@/views/department/part/labelset.vue'], resolve)
            }
        }]
    }/*,{
        path: '/doctorpart',
        name: '科室管理', //科室管理
        component: childView,
        isParent: true,
        redirect: '/doctorpart/list',
        children: [{
            path: '/doctorpart/list',
            name: '科室管理',
            component: resolve => {
                require(['@/views/department/doctorpart/list.vue'], resolve)
            }
        },{
            path: '/doctorpart/add',
            name: '科室管理',
            component: resolve => {
                require(['@/views/department/doctorpart/add.vue'], resolve)
            }
        },{
            path: '/doctorpart/update',
            name: '科室管理',
            component: resolve => {
                require(['@/views/department/doctorpart/update.vue'], resolve)
            }
        }]
    }*/]
  },{
    path: '/personal', //个人中心
    name: '个人中心',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/article',
        name: '文章管理', //文章管理
        component: childView,
        isParent: true,
        redirect: '/article/list',
        children: [{
            path: '/article/list',
            name: '文章列表',
            component: resolve => {
                require(['@/views/personal/article/list.vue'], resolve)
            }
        }, {
            path: '/article/add',
            name: '文章发布',
            component: resolve => {
                require(['@/views/personal/article/add.vue'], resolve)
            }
        }, {
            path: '/article/update',
            name: '文章修改',
            component: resolve => {
                require(['@/views/personal/article/update.vue'], resolve)
            }
        }]
    },{
        path: '/doctorarticle',
        name: '文章管理', //医生文章管理
        component: childView,
        isParent: true,
        redirect: '/doctorarticle/list',
        children: [{
            path: '/doctorarticle/list',
            name: '文章列表',
            component: resolve => {
                require(['@/views/personal/doctorarticle/list.vue'], resolve)
            }
        }, {
            path: '/doctorarticle/add',
            name: '文章添加',
            component: resolve => {
                require(['@/views/personal/doctorarticle/add.vue'], resolve)
            }
        }, {
            path: '/doctorarticle/update',
            name: '文章修改',
            component: resolve => {
                require(['@/views/personal/doctorarticle/update.vue'], resolve)
            }
        }]
    },{
        path: '/answer',
        name: '问答管理', //问答管理
        component: childView,
        isParent: true,
        redirect: '/answer/list',
        children: [{
            path: '/answer/list',
            name: '问答列表',
            component: resolve => {
                require(['@/views/personal/answer/list.vue'], resolve)
            }
        }, {
            path: '/answer/add',
            name: '问答添加',
            component: resolve => {
                require(['@/views/personal/answer/add.vue'], resolve)
            }
        }, {
            path: '/answer/update',
            name: '问答修改',
            component: resolve => {
                require(['@/views/personal/answer/update.vue'], resolve)
            }
        }]
    },{
        path: '/regime',
        name: '制度管理', //制度管理
        component: childView,
        isParent: true,
        redirect: '/regime/list',
        children: [{
            path: '/regime/list',
            name: '指南列表',
            component: resolve => {
                require(['@/views/personal/regime/list.vue'], resolve)
            }
        }, {
            path: '/regime/add',
            name: '指南添加',
            component: resolve => {
                require(['@/views/personal/regime/add.vue'], resolve)
            }
        }, {
            path: '/regime/update',
            name: '指南修改',
            component: resolve => {
                require(['@/views/personal/regime/update.vue'], resolve)
            }
        }]
    },{
        path: '/mobilepower',
        name: '移动端权限分配', //移动端权限分配
        component: childView,
        isParent: true,
        redirect: '/mobilepower/list',
        children: [{
            path: '/mobilepower/list',
            name: '移动端权限分配',
            component: resolve => {
                require(['@/views/personal/mobilepower/list.vue'], resolve)
            }
        }, {
            path: '/mobilepower/powerset',
            name: '权限设置',
            component: resolve => {
                require(['@/views/personal/mobilepower/powerset.vue'], resolve)
            }
        }]
    },{
        path: '/info',
        name: '个人资料', //个人资料
        component: childView,
        isParent: true,
        redirect: '/info/list',
        children: [{
            path: '/info/list',
            name: '个人资料详情',
            component: resolve => {
                require(['@/views/personal/info/list.vue'], resolve)
            }
        }, {
            path: '/info/update',
            name: '个人资料修改',
            component: resolve => {
                require(['@/views/personal/info/update.vue'], resolve)
            }
        }, {
            path: '/info/clinicadd',
            name: '科室添加',
            component: resolve => {
                require(['@/views/personal/info/clinicadd.vue'], resolve)
            }
        }, {
            path: '/info/clinicupdate',
            name: '科室修改',
            component: resolve => {
                require(['@/views/personal/info/clinicupdate.vue'], resolve)
            }
        }, {
            path: '/info/identity',
            name: '资料认证',
            component: resolve => {
                require(['@/views/personal/info/identity.vue'], resolve)
            }
        }, {
            path: '/info/identityupdate',
            name: '修改资料认证',
            component: resolve => {
                require(['@/views/personal/info/identityupdate.vue'], resolve)
            }
        }]
    },{
        path: '/pincome',
        name: '收入查看', //收入查看
        component: childView,
        isParent: true,
        redirect: '/pincome/list',
        children: [{
            path: '/pincome/list',
            name: '收入查看',
            component: resolve => {
                require(['@/views/personal/pincome/list.vue'], resolve)
            }
        }]
    },{
        path: '/identity',
        name: '认证资料', //认证资料
        component: childView,
        isParent: true,
        redirect: '/identity/list',
        children: [{
            path: '/identity/list',
            name: '认证资料',
            component: resolve => {
                require(['@/views/personal/identity/list.vue'], resolve)
            }
        }, {
            path: '/identity/add',
            name: '指南添加',
            component: resolve => {
                require(['@/views/personal/identity/add.vue'], resolve)
            }
        }, {
            path: '/identity/update',
            name: '指南修改',
            component: resolve => {
                require(['@/views/personal/identity/update.vue'], resolve)
            }
        }]
    },{
        path: '/arrange',
        name: '排班设置', //排班设置
        component: childView,
        isParent: true,
        redirect: '/arrange/list',
        children: [{
            path: '/arrange/list',
            name: '排班设置',
            component: resolve => {
                require(['@/views/personal/arrange/list.vue'], resolve)
            }
        }, {
            path: '/arrange/add',
            name: '排班添加',
            component: resolve => {
                require(['@/views/personal/arrange/add.vue'], resolve)
            }
        }, {
            path: '/arrange/update',
            name: '排班修改',
            component: resolve => {
                require(['@/views/personal/arrange/update.vue'], resolve)
            }
        }]
    },{
        path: '/case',
        name: '病例夹', //病例夹
        component: childView,
        isParent: true,
        redirect: '/case/list',
        children: [{
            path: '/case/list',
            name: '病例夹列表',
            component: resolve => {
                require(['@/views/personal/case/list.vue'], resolve)
            }
        }, {
            path: '/case/view',
            name: '病例详情',
            component: resolve => {
                require(['@/views/personal/case/view.vue'], resolve)
            }
        }]
    },{
        path: '/qualitycheck',
        name: '质控委员审核', //质控委员审核
        component: childView,
        isParent: true,
        redirect: '/qualitycheck/list',
        children: [{
            path: '/qualitycheck/list',
            name: '质控委员列表',
            component: resolve => {
                require(['@/views/personal/qualitycheck/list.vue'], resolve)
            }
        },{
            path: '/qualitycheck/view',
            name: '质控委员查看',
            component: resolve => {
                require(['@/views/personal/qualitycheck/view.vue'], resolve)
            }
        }]
    },{
        path: '/cliniccheck',
        name: '科室审核', //科室审核
        component: childView,
        isParent: true,
        redirect: '/cliniccheck/list',
        children: [{
            path: '/cliniccheck/list',
            name: '科室审核列表',
            component: resolve => {
                require(['@/views/personal/cliniccheck/list.vue'], resolve)
            }
        },{
            path: '/cliniccheck/view',
            name: '科室审核查看',
            component: resolve => {
                require(['@/views/personal/cliniccheck/view.vue'], resolve)
            }
        }]
    },{
        path: '/consultset',
        name: '咨询设置', //咨询设置
        component: childView,
        isParent: true,
        redirect: '/consultset/list',
        children: [{
            path: '/consultset/list',
            name: '咨询设置列表',
            component: resolve => {
                require(['@/views/personal/consultset/list.vue'], resolve)
            }
        },{
            path: '/consultset/add',
            name: '咨询设置添加',
            component: resolve => {
                require(['@/views/personal/consultset/add.vue'], resolve)
            }
        },{
            path: '/consultset/update',
            name: '咨询设置修改',
            component: resolve => {
                require(['@/views/personal/consultset/update.vue'], resolve)
            }
        }]
    },{
        path: '/appraise',
        name: '我的评价', //我的评价
        component: childView,
        isParent: true,
        redirect: '/appraise/list',
        children: [{
            path: '/appraise/list',
            name: '评价列表',
            component: resolve => {
                require(['@/views/personal/appraise/list.vue'], resolve)
            }
        },{
            path: '/appraise/view',
            name: '评价详情',
            component: resolve => {
                require(['@/views/personal/appraise/view.vue'], resolve)
            }
        }]
    },{
        path: '/doctorregime',
        name: '指南列表', //指南列表
        component: childView,
        isParent: true,
        redirect: '/doctorregime/list',
        children: [{
            path: '/doctorregime/list',
            name: '指南列表',
            component: resolve => {
                require(['@/views/personal/doctorregime/list.vue'], resolve)
            }
        },{
            path: '/doctorregime/view',
            name: '指南详情',
            component: resolve => {
                require(['@/views/personal/doctorregime/view.vue'], resolve)
            }
        }]
    },{
        path: '/message',
        name: '我的消息', //我的消息
        component: childView,
        isParent: true,
        redirect: '/message/list',
        children: [{
            path: '/message/list',
            name: '我的消息',
            component: resolve => {
                require(['@/views/personal/message/list.vue'], resolve)
            }
        }]
    },{
        path: '/incometype',
        name: '我的收款方式', //我的收款方式
        component: childView,
        isParent: true,
        redirect: '/incometype/list',
        children: [{
            path: '/incometype/list',
            name: '收款方式设置',
            component: resolve => {
                require(['@/views/personal/incometype/list.vue'], resolve)
            }
        }]
    },{
        path: '/chargeorder',
        name: '申诉审核订单', //申诉审核订单
        component: childView,
        isParent: true,
        redirect: '/chargeorder/list',
        children: [{
            path: '/chargeorder/list',
            name: '审核订单列表',
            component: resolve => {
                require(['@/views/personal/chargeorder/list.vue'], resolve)
            }
        }, {
            path: '/chargeorder/view',
            name: '审核订单详情',
            component: resolve => {
                require(['@/views/personal/chargeorder/view.vue'], resolve)
            }
        }]
    },{
        path: '/rebackorder',
        name: '退单审核订单', //退单审核订单
        component: childView,
        isParent: true,
        redirect: '/rebackorder/list',
        children: [{
            path: '/rebackorder/list',
            name: '退单审核列表',
            component: resolve => {
                require(['@/views/personal/rebackorder/list.vue'], resolve)
            }
        }, {
            path: '/rebackorder/view',
            name: '退单审核详情',
            component: resolve => {
                require(['@/views/personal/rebackorder/view.vue'], resolve)
            }
        }]
    }]
  },{
    path: '/tablecount', //统计报表
    name: '统计报表',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/ordercount',
        name: '订单统计', //订单统计
        component: childView,
        isParent: true,
        redirect: '/ordercount/list',
        children: [{
            path: '/ordercount/list',
            name: '统计列表',
            component: resolve => {
                require(['@/views/tablecount/ordercount/list.vue'], resolve)
            }
        }]
    },{
        path: '/income',
        name: '收入统计', //收入统计
        component: childView,
        isParent: true,
        redirect: '/income/list',
        children: [{
            path: '/income/list',
            name: '收入统计列表',
            component: resolve => {
                require(['@/views/tablecount/income/list.vue'], resolve)
            }
          }]
        },{
        path: '/companyincome',
        name: '公司收入查看', //公司收入查看
        component: childView,
        isParent: true,
        redirect: '/companyincome/list',
        children: [{
            path: '/companyincome/list',
            name: '公司收入查看',
            component: resolve => {
                require(['@/views/tablecount/companyincome/list.vue'], resolve)
            }
        }]
    },{
        path: '/appraisecount',
        name: '评价统计', //评价统计
        component: childView,
        isParent: true,
        redirect: '/appraisecount/list',
        children: [{
            path: '/appraisecount/list',
            name: '评价列表',
            component: resolve => {
                require(['@/views/tablecount/appraisecount/list.vue'], resolve)
            }
        }]
    },{
        path: '/rebackcount',
        name: '退单统计', //退单统计
        component: childView,
        isParent: true,
        redirect: '/rebackcount/list',
        children: [{
            path: '/rebackcount/list',
            name: '退单列表',
            component: resolve => {
                require(['@/views/tablecount/rebackcount/list.vue'], resolve)
            }
        }]
    },{
        path: '/transferrecord',
        name: '转账记录', //转账记录
        component: childView,
        isParent: true,
        redirect: '/transferrecord/list',
        children: [{
            path: '/transferrecord/list',
            name: '转账记录列表',
            component: resolve => {
                require(['@/views/tablecount/transferrecord/list.vue'], resolve)
            }
        }]
    },{
        path: '/flow',
        name: '流量查看', //流量查看
        component: childView,
        isParent: true,
        redirect: '/flow/list',
        children: [{
            path: '/flow/list',
            name: '流量统计',
            component: resolve => {
                require(['@/views/tablecount/flow/list.vue'], resolve)
            }
        }]
    }]
  },{
    path: '/quality', //质控管理
    name: '质控管理',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/qualityuser',
        name: '人员管理', //人员管理
        component: childView,
        isParent: true,
        redirect: '/qualityuser/list',
        children: [{
            path: '/qualityuser/list',
            name: '质控人员列表',
            component: resolve => {
                require(['@/views/quality/qualityuser/list.vue'], resolve)
            }
        },{
            path: '/qualityuser/add',
            name: '人员添加',
            component: resolve => {
                require(['@/views/quality/qualityuser/add.vue'], resolve)
            }
        }]
    },{
        path: '/qualityorder',
        name: '质控订单', //质控订单
        component: childView,
        isParent: true,
        redirect: '/qualityorder/list',
        children: [{
            path: '/qualityorder/list',
            name: '订单列表',
            component: resolve => {
                require(['@/views/quality/qualityorder/list.vue'], resolve)
            }
        },{
            path: '/qualityorder/view',
            name: '评价详情',
            component: resolve => {
                require(['@/views/quality/qualityorder/view.vue'], resolve)
            }
        },{
            path: '/qualityorder/consultdetail',
            name: '咨询订单详情',
            component: resolve => {
                require(['@/views/quality/qualityorder/consultdetail.vue'], resolve)
            }
        }]
    },{
        path: '/complain',
        name: '申诉管理', //申诉管理
        component: childView,
        isParent: true,
        redirect: '/complain/list',
        children: [{
            path: '/complain/list',
            name: '申诉管理列表',
            component: resolve => {
                require(['@/views/quality/complain/list.vue'], resolve)
            }
        }]
    }]
  },{
    path: '/systemuse', //系统应用
    name: '系统应用',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/systemlog'
        ,
        name: '系统日志', //系统日志
        component: childView,
        isParent: true,
        redirect: '/systemlog/list',
        children: [{
            path: '/systemlog/list',
            name: '日志列表',
            component: resolve => {
                require(['@/views/systemuse/systemlog/list.vue'], resolve)
            }
        }]
    },{
        path: '/doctorlog',
        name: '医生设置日志', //医生设置日志
        component: childView,
        isParent: true,
        redirect: '/doctorlog/list',
        children: [{
            path: '/doctorlog/list',
            name: '日志列表',
            component: resolve => {
                require(['@/views/systemuse/doctorlog/list.vue'], resolve)
            }
        }]
    },{
        path: '/articlelog',
        name: '文章文答日志', //文章文答日志
        component: childView,
        isParent: true,
        redirect: '/articlelog/list',
        children: [{
            path: '/articlelog/list',
            name: '日志列表',
            component: resolve => {
                require(['@/views/systemuse/articlelog/list.vue'], resolve)
            }
        }]
    }]
  },{
    path: '/datacenter', //数据中心
    name: '数据中心',
    component: body,
    iconCls: 'icon-file',
    children: [{
        path: '/operatedata'
        ,
        name: '运营数据', //运营数据报表
        component: childView,
        isParent: true,
        redirect: '/operatedata/list',
        children: [{
            path: '/operatedata/list',
            name: '运营数据报表',
            component: resolve => {
                require(['@/views/datacenter/operatedata/list.vue'], resolve)
            }
        }]
    }]
  }
]