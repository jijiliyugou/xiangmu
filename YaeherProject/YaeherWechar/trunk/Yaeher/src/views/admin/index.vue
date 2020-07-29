<template>
    <div class="admin-index padding-top">
        <mt-header fixed title="待办"></mt-header>
        <div class="p-content content">
            <ul class="user-list doctor-user-list">
                <li>
                    <router-link to="/admin-doctor" class="user-href">
                        <div class="flex">
                            <div class="user-title">医生注册信息审核</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/chargeback-list" class="user-href">
                        <div class="flex">
                            <div class="user-title">退单审核</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>    
        </div>
        <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="nav1" href="#/index-admin">
                <img slot="icon" src="../../assets/image/question-icon.png">
                待办
            </mt-tab-item>
            <mt-tab-item id="nav2" href="#/financing">
                <img slot="icon" src="../../assets/image/class-icon.png">
                财务查看
            </mt-tab-item>
            <mt-tab-item id="nav3" href="#/admin-user">
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
            selected: 'nav1'
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.admin-index {
    .doctor-user-list {
        margin-bottom: 10px;
        .user-title {
            padding-left: 0;
        }
    }
}

</style>