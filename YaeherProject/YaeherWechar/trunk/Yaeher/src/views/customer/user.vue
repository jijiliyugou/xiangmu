<template>
    <div class="customer-user padding-top">
        <mt-header fixed title="个人中心"></mt-header>
        <div class="p-content">
            <ul class="user-list doctor-user-list">
                <li>
                    <router-link to="/customer-order" class="user-href">
                        <div class="flex">
                            <div class="user-title">订单查看</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/subject" class="user-href">
                        <div class="flex">
                            <div class="user-title">科室管理</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/manage-doctor" class="user-href">
                        <div class="flex">
                            <div class="user-title">医生管理</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/audit-clinic-list" class="user-href">
                        <div class="flex">
                            <div class="user-title">医生科室审核</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <!-- <li>
                    <a href="https://m.qlchat.com/wechat/page/live/210000155031160" class="user-href">
                        <div class="flex">
                            <div class="user-title">听课程</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </a>
                </li> -->
            </ul>    
        </div>
        <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="nav1" href="#/index-customer">
                <img slot="icon" src="../../assets/image/question-icon.png">
                待审核
            </mt-tab-item>
            <mt-tab-item id="nav2" href="#/information-list">
                <img slot="icon" src="../../assets/image/class-icon.png">
                我的消息
            </mt-tab-item>
            <mt-tab-item id="nav3" href="#/customer-user">
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
            selected: 'nav3'
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.customer-user {
    .doctor-user-list {
        margin-bottom: 10px;
        .user-title {
            padding-left: 0;
        }
    }
}

</style>