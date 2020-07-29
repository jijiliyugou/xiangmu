<template>
    <div class="control-user padding-top">
        <mt-header fixed title="个人中心"></mt-header>
        <div class="p-content">
            <ul class="user-list doctor-user-list">
                <li>
                    <router-link :to="{path: '/complaint-management', query: {title: '申诉管理'}}" class="user-href">
                        <div class="flex">
                            <div class="user-title">申诉管理</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link :to="{path: '/complaint-management', query: {title: '退单列表'}}" class="user-href">
                        <div class="flex">
                            <div class="user-title">退单列表</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/control-list" class="user-href">
                        <div class="flex">
                            <div class="user-title">质控委员列表</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/add-control" class="user-href">
                        <div class="flex">
                            <div class="user-title">添加质控委员</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/audit-control" class="user-href">
                        <div class="flex">
                            <div class="user-title">质控委员审核</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/control-report" class="user-href">
                        <div class="flex">
                            <div class="user-title">报表统计</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>    
        </div>
        <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="nav1" href="#/index-control">
                <img slot="icon" src="../../assets/image/question-icon.png">
                评价统计
            </mt-tab-item>
            <mt-tab-item id="nav2" href="#/doctor-list-control">
                <img slot="icon" src="../../assets/image/class-icon.png">
                医生查看
            </mt-tab-item>
            <mt-tab-item id="nav3" href="#/audit-article">
                <img slot="icon" src="../../assets/image/class-icon.png">
                文章管理
            </mt-tab-item>
            <mt-tab-item id="nav4" href="#/control-user">
                <img slot="icon" src="../../assets/image/user-icon.png">
                个人中心
            </mt-tab-item>
        </mt-tabbar>
    </div>
</template>

<script>
export default {
    data () {
        return {
            selected: 'nav4'
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.control-user {
    .doctor-user-list {
        margin-bottom: 10px;
        .user-title {
            padding-left: 0;
        }
    }
}

</style>